using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RecursoCoste { Mana, Sanidad }

[System.Serializable]
public struct Accion
{
    public string nombre;
    public bool estatico;
    public bool objetivoEsElEquipo;

    public string mensaje;
    public int argumento;

    public string animacionTrigger;
    public string efectoSecundario;
    public int costoMana;
    public RecursoCoste tipoCoste;
}

public class Luchador : MonoBehaviour
{
    public string nombre;
    public int vida;
    public int mana;
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
    }

    public IEnumerator EjecutarAccion(Accion accion, Luchador objetivo)
    {
        Debug.Log($"[{nombre}] ejecuta {accion.nombre} sobre {objetivo.nombre}");

        // Recurso
        switch (accion.tipoCoste)
        {
            case RecursoCoste.Mana: mana -= accion.costoMana; break;
            case RecursoCoste.Sanidad: CambiarSanidad(-accion.costoMana); break;
        }

        if (accion.objetivoEsElEquipo) objetivo = this;

        if (accion.nombre == "GrimFandango")
        {
            saltarSiguienteTurno = true;
        }

        if (accion.estatico)
        {
            EjecutarEfecto(accion, objetivo);
        }
        else
        {
            Vector3 origen = transform.position;
            transform.LookAt(objetivo.transform.position);
            nv.SetDestination(objetivo.transform.position);

            while (Vector3.Distance(transform.position, objetivo.transform.position) > 2f)
                yield return null;

            EjecutarEfecto(accion, objetivo);

            transform.LookAt(origen);
            nv.SetDestination(origen);

            while (Vector3.Distance(transform.position, origen) > 0.1f)
                yield return null;
        }
    }

    private void EjecutarEfecto(Accion accion, Luchador objetivo)
    {
        switch (accion.mensaje)
        {
            case "CambiarVida":
                objetivo.CambiarVida(accion.argumento + bonusDaño);
                break;
            case "CambiarMana":
                objetivo.CambiarMana(accion.argumento);
                break;
            case "CambiarSanidad":
                objetivo.CambiarSanidad(accion.argumento);
                break;
            case "AplicarEfecto":
                TipoEfecto tipo = Enum.TryParse(accion.efectoSecundario, out TipoEfecto resultado) ? resultado : TipoEfecto.Sangrado;
                var efecto = new EfectoActivo
                {
                    nombre = accion.efectoSecundario,
                    tipo = tipo,
                    modificador = accion.argumento,
                    duracionTurnos = 3
                };
                objetivo.efectosActivos.Add(efecto);
                break;
        }

        if (!string.IsNullOrEmpty(accion.animacionTrigger))
            anim?.SetTrigger(accion.animacionTrigger);
    }

    public void AplicarEfectosPorTurno()
    {
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

        // Aumentar daño recibido si tiene Furia activa
        if (cantidad < 0 && estadoEspecial.FuriaRecibidaExtra > 0)
        {
            cantidad -= estadoEspecial.FuriaRecibidaExtra;
        }

        vida += cantidad;

        if (estadoEspecial.ReflejarDanioA != null && cantidad < 0)
        {
            int dañoReflejado = Mathf.CeilToInt(Mathf.Abs(cantidad) * 1f); // 100%
            estadoEspecial.ReflejarDanioA.CambiarVida(-dañoReflejado);
            Debug.Log($"{estadoEspecial.ReflejarDanioA.nombre} sufre {dañoReflejado} por Evil Dead (reflejo).");

        }

        if (vida <= 0 && sigueVivo)
        {
            sigueVivo = false;
            gameObject.SetActive(false);
            Debug.Log($"{nombre} ha sido derrotado.");
        }

        if (estadoEspecial.Critico && cantidad < 0 && UnityEngine.Random.value < 0.4f)
        {
            cantidad *= 2;
            Debug.Log($"{nombre} hizo un CRÍTICO!");
        }

    }


    public void CambiarMana(int cantidad)
    {
        mana += cantidad;
    }

    public void CambiarSanidad(int cantidad)
    {
        sanidad += cantidad;
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

    public List<Luchador> ObtenerEnemigosCercanos()
    {
        return new List<Luchador>(FindObjectsOfType<Luchador>())
            .FindAll(l => l.Aliado != this.Aliado && l.sigueVivo);
    }

 


}
