// Clase principal para los personajes que participan en combate, ya sean aliados o enemigos
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Tipos de recurso que puede consumir una acción
public enum RecursoCoste { Sanidad }

// Representa una acción que una carta o luchador puede ejecutar
[System.Serializable]
public struct Accion
{
    public string nombre;
    public bool estatico;                  // Si es estatico, no requiere movimiento
    public bool objetivoEsElEquipo;        // Si el objetivo es uno mismo

    public string mensaje;                 // Tipo de efecto (daño, curación, etc.)
    public int argumento;                  // Valor del efecto

    public string animacionTrigger;        // Animación a reproducir
    public string efectoSecundario;        // Nombre del efecto adicional
    public int costoMana;                  // Costo de sanidad
    public RecursoCoste tipoCoste;         // Tipo de recurso usado
}

public class Luchador : MonoBehaviour
{
    public string nombre;
    public int vida;
    public int vidaMaxima;
    public int sanidad = 100;
    public int bonusDaño = 0;

    public bool Aliado;
    public bool sigueVivo = true;

    public CardCollection cartasDisponibles;
    public List<Accion> Acciones;
    public List<EfectoActivo> efectosActivos = new();
    public EstadoEspecial estadoEspecial = new();

    private Animator anim;
    public NavMeshAgent nv;

    public bool saltarSiguienteTurno = false;
    public EfectoActivo.FuriaFocalizada furiaFocalizada;

    private void Start()
    {
        anim = GetComponent<Animator>();
        nv = GetComponent<NavMeshAgent>();
        nv.updateRotation = false;

        // Ajusta vida máxima si no está definida
        if (vidaMaxima <= 0)
            vidaMaxima = vida;
    }

    // Ejecuta una acción principal y secundaria sobre un objetivo
    public IEnumerator EjecutarAccion(Accion accion, Luchador objetivo, Accion? accionSecundaria = null)
    {
        Debug.Log($"[{nombre}] ejecuta {accion.nombre} sobre {objetivo.nombre}");

        if (accion.tipoCoste == RecursoCoste.Sanidad)
            CambiarSanidad(-accion.costoMana);

        if (accion.objetivoEsElEquipo)
            objetivo = this;

        if (accion.nombre == "GrimFandango")
            saltarSiguienteTurno = true;

        // Si es una acción estática, se aplica sin moverse
        if (accion.estatico)
        {
            EjecutarEfecto(accion, objetivo);
            if (accionSecundaria.HasValue)
            {
                var sec = accionSecundaria.Value;
                Luchador objetivoSec = sec.objetivoEsElEquipo ? this : objetivo;
                EjecutarEfecto(sec, objetivoSec);
            }
        }
        else
        {
            Vector3 origen = transform.position;
            transform.LookAt(objetivo.transform.position);
            nv.SetDestination(objetivo.transform.position);

            while (Vector3.Distance(transform.position, objetivo.transform.position) > 2f)
                yield return null;

            EjecutarEfecto(accion, objetivo);

            if (accionSecundaria.HasValue)
            {
                var sec = accionSecundaria.Value;
                Luchador objetivoSec = sec.objetivoEsElEquipo ? this : objetivo;
                EjecutarEfecto(sec, objetivoSec);
            }

            transform.LookAt(origen);
            nv.SetDestination(origen);

            while (Vector3.Distance(transform.position, origen) > 0.1f)
                yield return null;
        }
    }

    // Aplica el efecto de una acción sobre un objetivo
    private void EjecutarEfecto(Accion accion, Luchador objetivo)
    {
        switch (accion.mensaje)
        {
            case "CambiarVida":
                objetivo.CambiarVida(accion.argumento + bonusDaño);
                break;

            case "CambiarVidaAleatorio":
                int dañoAleatorio = UnityEngine.Random.Range(-4, -1);
                objetivo.CambiarVida(dañoAleatorio + bonusDaño);
                Debug.Log($"{nombre} causa daño aleatorio de {dañoAleatorio} a {objetivo.nombre}");
                break;

            case "CurarORevivir":
                if (!objetivo.sigueVivo)
                {
                    objetivo.Revivir(accion.argumento);
                    objetivo.CambiarSanidad(-2);
                    Debug.Log($"{objetivo.nombre} ha sido revivido y pierde 2 de sanidad.");
                }
                else
                {
                    objetivo.CambiarVida(accion.argumento);
                    if (UnityEngine.Random.value < 0.5f)
                    {
                        objetivo.CambiarSanidad(-1);
                        Debug.Log($"{objetivo.nombre} fue curado pero perdió 1 de sanidad.");
                    }
                }
                break;

            case "RobarSalud":
                int robo = Mathf.Abs(accion.argumento);
                objetivo.CambiarVida(-robo);
                this.CambiarVida(robo);
                Debug.Log($"{nombre} roba {robo} HP de {objetivo.nombre}");
                break;

            case "RestaurarTodo":
                objetivo.RestaurarVidaTotal();
                Debug.Log($"{objetivo.nombre} restauró su vida completamente.");
                break;

            case "AplicarEfectoGlobal":
                if (!Enum.TryParse<TipoEfecto>(accion.efectoSecundario, out TipoEfecto efectoGlobal))
                {
                    Debug.LogError($"[ERROR] Efecto global inválido: {accion.efectoSecundario}");
                    return;
                }

                foreach (var enemigo in UnityEngine.Object.FindObjectsOfType<Luchador>())
                {
                    if (enemigo.Aliado != this.Aliado && enemigo.sigueVivo)
                    {
                        if (efectoGlobal == TipoEfecto.DanioEnArea)
                        {
                            enemigo.CambiarVida(accion.argumento);
                            Debug.Log($"{enemigo.nombre} recibe daño global inmediato de {accion.argumento}");
                        }
                        else
                        {
                            var efecto = new EfectoActivo
                            {
                                nombre = efectoGlobal.ToString(),
                                tipo = efectoGlobal,
                                modificador = accion.argumento,
                                duracionTurnos = 3
                            };
                            enemigo.efectosActivos.Add(efecto);
                            Debug.Log($"{enemigo.nombre} recibe efecto global {efectoGlobal}");
                        }
                    }
                }
                break;

            case "AplicarEfecto":
                if (accion.efectoSecundario == "ConfusionGlobal")
                {
                    foreach (var enemigo in FindObjectsOfType<Luchador>())
                    {
                        if (enemigo.Aliado != this.Aliado && enemigo.sigueVivo)
                        {
                            enemigo.estadoEspecial.Confusion = true;
                            Debug.Log($"{enemigo.nombre} está confundido por {nombre}.");
                        }
                    }
                    break;
                }

                TipoEfecto tipo;
                if (accion.efectoSecundario == "GlitchRandom")
                {
                    var opciones = new[] { TipoEfecto.Sangrado, TipoEfecto.Paralizado, TipoEfecto.Confusion };
                    tipo = opciones[UnityEngine.Random.Range(0, opciones.Length)];
                    Debug.Log($"Glitch Beat aplica efecto aleatorio: {tipo}");
                }
                else
                {
                    if (!Enum.TryParse<TipoEfecto>(accion.efectoSecundario, out tipo))
                    {
                        Debug.LogError($"[ERROR] No se pudo convertir '{accion.efectoSecundario}' en TipoEfecto.");
                        return;
                    }
                    else
                    {
                        Debug.Log($"[DEBUG] Tipo de efecto detectado: {tipo}");
                    }
                }

                var nuevoEfecto = new EfectoActivo
                {
                    nombre = tipo.ToString(),
                    tipo = tipo,
                    modificador = accion.argumento,
                    duracionTurnos = 3
                };
                Debug.Log($"Añadiendo efecto {nuevoEfecto.nombre}");
                objetivo.efectosActivos.Add(nuevoEfecto);
                break;
        }

        if (!string.IsNullOrEmpty(accion.animacionTrigger))
            anim?.SetTrigger(accion.animacionTrigger);
    }

    public void RestaurarVidaTotal()
    {
        this.vida = vidaMaxima;
    }

    public void AplicarEfectosPorTurno()
    {
        Debug.Log($"AplicarEfectos {efectosActivos.Count}");
        estadoEspecial.Reiniciar();
        for (int i = efectosActivos.Count - 1; i >= 0; i--)
        {
            var efecto = efectosActivos[i];
            efecto.AplicarEfectoPorTurno(this, this);

            if (efecto.Expirado)
            {
                efectosActivos.RemoveAt(i);
                Debug.Log($"{nombre} pierde el efecto: {efecto.nombre}");
            }
        }
    }

    public void CambiarVida(int cantidad)
    {
        if (estadoEspecial.Asqueado && cantidad > 0)
        {
            Debug.Log($"{nombre} no puede curarse por asqueado.");
            return;
        }

        if (cantidad < 0 && estadoEspecial.FuriaRecibidaExtra > 0)
            cantidad -= estadoEspecial.FuriaRecibidaExtra;

        if (estadoEspecial.Critico && cantidad < 0 && UnityEngine.Random.value < 0.4f)
        {
            cantidad *= 2;
            Debug.Log($"{nombre} hizo un CRÍTICO!");
        }

        vida += cantidad;

        if (estadoEspecial.ReflejarDanioA != null && cantidad < 0)
        {
            int dañoReflejado = Mathf.CeilToInt(Mathf.Abs(cantidad));
            estadoEspecial.ReflejarDanioA.CambiarVida(-dañoReflejado);
            Debug.Log($"{estadoEspecial.ReflejarDanioA.nombre} sufre {dañoReflejado} por reflejo de daño.");
        }

        if (vida <= 0 && sigueVivo)
        {
            if (estadoEspecial.ResucitarUnaVez)
            {
                estadoEspecial.ResucitarUnaVez = false;
                Revivir(10);
            }
            else
            {
                sigueVivo = false;
                gameObject.SetActive(false);
                Debug.Log($"{nombre} ha sido derrotado.");
            }
        }
    }

    public void CambiarSanidad(int cantidad)
    {
        sanidad += cantidad;
        sanidad = Mathf.Clamp(sanidad, 0, 100);
        if (sanidad <= 0)
            Debug.Log($"{nombre} ha perdido la cordura.");
    }

    public void Revivir(int cantidad)
    {
        if (!sigueVivo)
        {
            vida = cantidad;
            sigueVivo = true;
            gameObject.SetActive(true);
            Debug.Log($"{nombre} ha revivido con {cantidad} de vida.");
        }
    }

    // Obtiene enemigos vivos para posibles efectos o acciones
    public List<Luchador> ObtenerEnemigosCercanos()
    {
        return new List<Luchador>(FindObjectsOfType<Luchador>())
            .FindAll(l => l.Aliado != this.Aliado && l.sigueVivo);
    }
}
