// Clase que representa un luchador en el combate, puede ser jugador o enemigo
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Tipos de recursos que puede usar una carta (por ahora solo Sanidad)
public enum RecursoCoste { Sanidad }

// Estructura que define una acci�n de combate
[System.Serializable]
public struct Accion
{
    public string nombre;
    public bool estatico;                  // Si requiere desplazamiento o no
    public bool objetivoEsElEquipo;        // Si se aplica al propio lanzador
    public string mensaje;                 // Tipo de efecto (CambiarVida, CurarORevivir, etc.)
    public int argumento;                  // Valor del efecto
    public string animacionTrigger;       // Trigger de animaci�n
    public string efectoSecundario;        // Efecto adicional a aplicar
    public int costoMana;                 // Costo en sanidad
    public RecursoCoste tipoCoste;        // Tipo de recurso utilizado
}

public class Luchador : MonoBehaviour
{
    public string nombre;
    public int vida;
    public int vidaMaxima;
    public int sanidad;
    public int sanidadMaxima;
    public int bonusDa�o = 0;
    public bool ReflejoUnicoActivo = false;

    public bool Aliado;                   // Si es parte del equipo del jugador
    public bool sigueVivo = true;

    public CardCollection cartasDisponibles; // Cartas que puede usar este luchador
    public List<Accion> Acciones;
    public List<EfectoActivo> efectosActivos = new();
    public EstadoEspecial estadoEspecial = new();

    private Animator anim;
    public NavMeshAgent nv;

    public bool saltarSiguienteTurno = false;
    public EfectoActivo.FuriaFocalizada furiaFocalizada;

    [Header("Animaciones Personalizadas")]
    public string animacionRecibirDa�o;
    public string animacionMuerte;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim = GetComponentInChildren<Animator>();
        nv = GetComponent<NavMeshAgent>();
        nv.updateRotation = false;
        if (vidaMaxima <= 0)
            vidaMaxima = vida;

        // Evita colisiones entre luchadores
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Luchadores"), LayerMask.NameToLayer("Luchadores"), true);
        nv.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

        RestaurarEstadoDesdePersistencia();
    }

    // Corrutina que ejecuta la acci�n de combate sobre un objetivo
    public IEnumerator EjecutarAccion(Accion accion, Luchador objetivo, Accion? accionSecundaria = null, Luchador atacante = null)
    {
        Debug.Log($"[{nombre}] ejecuta {accion.nombre} sobre {objetivo.nombre}");

        if (accion.tipoCoste == RecursoCoste.Sanidad)
            CambiarSanidad(-accion.costoMana);

        if (accion.objetivoEsElEquipo)
            objetivo = this;

        if (accion.nombre == "GrimFandango")
            saltarSiguienteTurno = true;

        // Si no hay movimiento necesario
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
            // Guardar posici�n original
            Vector3 origen = transform.position;
            Quaternion rotacionOriginal = transform.rotation;
            transform.LookAt(objetivo.transform.position);

            // Calcular posici�n frente al objetivo
            Vector3 offset = (objetivo.transform.position - transform.position).normalized * 1.5f;
            Vector3 destino = objetivo.transform.position - offset;

            // Mover hacia el enemigo
            nv.SetDestination(destino);

            // Esperar a llegar al destino
            while (Vector3.Distance(transform.position, destino) > 0.5f)
                yield return null;

            // Asegurar que est� mirando al objetivo
            transform.LookAt(objetivo.transform.position);

            // Esperar 0.1s por seguridad de sincronizaci�n
            yield return new WaitForSeconds(0.1f);

            // Ejecutar animaci�n si corresponde
            if (!string.IsNullOrEmpty(accion.animacionTrigger))
            {
                anim.SetTrigger(accion.animacionTrigger);

                // Esperar a que termine la animaci�n
                yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !anim.IsInTransition(0));
            }

            // Ejecutar efecto
            EjecutarEfecto(accion, objetivo, atacante);

            if (accionSecundaria.HasValue)
            {
                var sec = accionSecundaria.Value;
                Luchador objetivoSec = sec.objetivoEsElEquipo ? this : objetivo;
                EjecutarEfecto(sec, objetivoSec);
            }

            yield return new WaitForSeconds(1f);

            // Volver a posici�n original
            transform.LookAt(origen);
            nv.SetDestination(origen);

            while (Vector3.Distance(transform.position, origen) > 0.1f)
                yield return null;

            
            transform.rotation = rotacionOriginal;
        }



    }

    // Ejecuta el efecto de una acci�n en el objetivo
    private void EjecutarEfecto(Accion accion, Luchador objetivo, Luchador atacante = null)
    {
        switch (accion.mensaje)
        {
            case "CambiarVida":
                if (atacante != null)
                {
                    Debug.Log(atacante.name);
                    if (objetivo.ReflejoUnicoActivo == true)
                    {
                        atacante.CambiarVida(accion.argumento + bonusDa�o);
                        objetivo.ReflejoUnicoActivo = false;
                    }
                    else
                    {
                        objetivo.CambiarVida(accion.argumento + bonusDa�o);
                    }
                }
                else
                {
                    objetivo.CambiarVida(accion.argumento + bonusDa�o);
                }
                


                break;

            case "CambiarVidaAleatorio":
                int min = -4;
                int max = -1;
                if (int.TryParse(accion.argumento.ToString(), out int a) &&
                    int.TryParse(accion.efectoSecundario, out int b))
                {
                    min = Mathf.Min(a, b);
                    max = Mathf.Max(a, b);
                }

                int da�o = UnityEngine.Random.Range(min, max + 1);
                objetivo.CambiarVida(da�o + bonusDa�o);
                Debug.Log($"{nombre} causa da�o aleatorio de {da�o} a {objetivo.nombre}");
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
                        Debug.Log($"{objetivo.nombre} fue curado pero perdi� 1 de sanidad.");
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
                Debug.Log($"{objetivo.nombre} restaur� su vida completamente.");
                break;

            case "AplicarEfectoGlobal":
                if (!Enum.TryParse<TipoEfecto>(accion.efectoSecundario, out TipoEfecto efectoGlobal))
                {
                    Debug.LogError($"[ERROR] Efecto global inv�lido: {accion.efectoSecundario}");
                    return;
                }

                foreach (var enemigo in UnityEngine.Object.FindObjectsOfType<Luchador>())
                {
                    if (enemigo.Aliado != this.Aliado && enemigo.sigueVivo)
                    {
                        var efecto = new EfectoActivo
                        {
                            nombre = efectoGlobal.ToString(),
                            tipo = efectoGlobal,
                            modificador = accion.argumento,
                            duracionTurnos = 3,
                            lanzador = this //�Esto es lo que faltaba!
                        };
                        if (efectoGlobal == TipoEfecto.DanioEnArea)
                        {
                            if (enemigo.Aliado != this.Aliado && enemigo.sigueVivo)
                            {
                                enemigo.CambiarVida(-accion.argumento);
                                Debug.Log($"{enemigo.nombre} recibi� {accion.argumento} de da�o en �rea directamente.");
                            }
                        }

                        else
                        {
                            enemigo.efectosActivos.Add(efecto);
                            Debug.Log($"{enemigo.nombre} recibe efecto global {efectoGlobal}");
                        }
                        Debug.Log($"{enemigo.nombre} recibe efecto global {efectoGlobal}");
                    }
                }
                break;

            case "AplicarEfecto":
                TipoEfecto tipo;
                if (accion.efectoSecundario == "GlitchRandom")
                {
                    var opciones = new[] { TipoEfecto.Sangrado, TipoEfecto.Paralizado, TipoEfecto.Confusion };
                    tipo = opciones[UnityEngine.Random.Range(0, opciones.Length)];
                    Debug.Log($"Glitch Beat aplica efecto aleatorio: {tipo}");
                }
                else if (!Enum.TryParse<TipoEfecto>(accion.efectoSecundario, out tipo))
                {
                    Debug.LogError($"[ERROR] No se pudo convertir '{accion.efectoSecundario}' en TipoEfecto.");
                    return;
                }

                var nuevoEfecto = new EfectoActivo
                {
                    nombre = tipo.ToString(),
                    tipo = tipo,
                    modificador = accion.argumento,
                    duracionTurnos = 3,
                    lanzador = this
                };

                if (tipo == TipoEfecto.FuriaSanidad)
                {
                    int sanidadPerdida = objetivo.sanidadMaxima - objetivo.sanidad;
                    nuevoEfecto.bonusFuriaSanidad = sanidadPerdida;
                    objetivo.bonusDa�o += sanidadPerdida;
                    Debug.Log($"{objetivo.nombre} obtiene {sanidadPerdida} de da�o por Furia Sanidad (aplicado una vez).");
                }

                Debug.Log($"A�adiendo efecto {nuevoEfecto.nombre}");
                objetivo.efectosActivos.Add(nuevoEfecto);
                break;

            case "CurarYLimpiar":
                // Curar vida
                objetivo.CambiarVida(accion.argumento);

                // Eliminar todos los efectos activos
                objetivo.efectosActivos.Clear();
                objetivo.estadoEspecial.Reiniciar();

                Debug.Log($"{objetivo.nombre} fue curado y todos sus efectos negativos fueron eliminados.");
                break;
            case "ReflejarDanio":

                objetivo.ReflejoUnicoActivo = true;

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
                if (efecto.tipo == TipoEfecto.CompartirDa�o)
                {
                    Debug.Log($"{nombre} ya no refleja da�o a {estadoEspecial.ReflejarDanioA?.nombre}");
                    estadoEspecial.ReflejarDanioA = null;
                }

                if (efecto.tipo == TipoEfecto.FuriaSanidad && efecto.bonusFuriaSanidad > 0)
                {
                    bonusDa�o -= efecto.bonusFuriaSanidad;
                    Debug.Log($"{nombre} pierde {efecto.bonusFuriaSanidad} de Furia Sanidad al expirar el efecto.");
                }

                if (efecto.tipo == TipoEfecto.Furia && efecto.aplicado)
                {
                    bonusDa�o -= efecto.modificador;
                    Debug.Log($"{nombre} pierde el bonus de Furia (+{efecto.modificador}).");
                }

                efectosActivos.RemoveAt(i);
                Debug.Log($"{nombre} pierde el efecto: {efecto.nombre}");
            }


        }
    }

    public void CambiarVida(int cantidad)
    {
        if (cantidad < 0 && estadoEspecial.FuriaRecibidaExtra > 0)
            cantidad -= estadoEspecial.FuriaRecibidaExtra;

        if (estadoEspecial.Critico && cantidad < 0)
        {
            if (efectosActivos.Exists(e => e.tipo == TipoEfecto.Critico && e.modificador == 999))
            {
                cantidad *= 2;
                Debug.Log($"{nombre} hizo un CR�TICO FORZADO!");
            }
            else if (UnityEngine.Random.value < 0.4f)
            {
                cantidad *= 2;
                Debug.Log($"{nombre} hizo un CR�TICO!");
            }
        }
        
        if (cantidad < 0 && anim != null && !string.IsNullOrEmpty(animacionRecibirDa�o))
            anim.SetTrigger(animacionRecibirDa�o);

        vida += cantidad;

        if (estadoEspecial.ReflejarDanioA != null && cantidad < 0)
        {
            int da�oReflejado = Mathf.CeilToInt(Mathf.Abs(cantidad));
            estadoEspecial.ReflejarDanioA.CambiarVida(-da�oReflejado);
            Debug.Log($"{estadoEspecial.ReflejarDanioA.nombre} sufre {da�oReflejado} por reflejo de da�o.");
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
                anim.SetTrigger(animacionMuerte);
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

    private void RestaurarEstadoDesdePersistencia()
    {
        if (!Aliado || EstadoAliados.Instancia == null) return;

        var datos = EstadoAliados.Instancia.estados.Find(e => e.nombre == nombre);
        if (datos != null)
        {
            vida = datos.vida;
            sanidad = datos.sanidad;
            sigueVivo = datos.sigueVivo;
            gameObject.SetActive(sigueVivo);

            Debug.Log($"[{nombre}] restaurado desde persistencia: Vida={vida}, Sanidad={sanidad}, Vivo={sigueVivo}");
        }
    }

}
