using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RecursoCoste
{
    Mana,
    Sanidad
}

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

public enum TipoEfecto
{
    DañoPorTurno,
    CurarPorTurno,
    ModificarDaño
}

[System.Serializable]
public class EfectoActivo
{
    public string nombre;
    public int duracionTurnos;
    public int modificador;
    public TipoEfecto tipo;

    public void AplicarEfectoPorTurno(Luchador objetivo)
    {
        switch (tipo)
        {
            case TipoEfecto.DañoPorTurno:
                objetivo.CambiarVida(-modificador);
                Debug.Log($"{objetivo.nombre} sufre {modificador} de daño por turno.");
                break;
            case TipoEfecto.CurarPorTurno:
                objetivo.CambiarVida(modificador);
                Debug.Log($"{objetivo.nombre} se cura {modificador} por turno.");
                break;
            case TipoEfecto.ModificarDaño:
                objetivo.bonusDaño += modificador;
                Debug.Log($"{objetivo.nombre} gana {modificador} de bonus de daño.");
                break;
        }
    }

    public bool EsPermanente => duracionTurnos <= 0;
}

public class Luchador : MonoBehaviour
{
    #region Estado

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

    private Animator anim;
    public NavMeshAgent nv;

    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();
        nv = GetComponent<NavMeshAgent>();
        nv.updateRotation = false;
    }

    public void EjecutarAccionDesdeCarta(Accion accion, Luchador objetivo)
    {
        StartCoroutine(EjecutarAccion(accion, objetivo));
    }

    public IEnumerator EjecutarAccion(Accion accion, Luchador objetivo)
    {
        Debug.Log($"[{nombre}] ejecuta {accion.nombre} sobre {objetivo.nombre}");

        // Descontar recurso
        switch (accion.tipoCoste)
        {
            case RecursoCoste.Mana:
                mana -= accion.costoMana;
                break;
            case RecursoCoste.Sanidad:
                CambiarSanidad(-accion.costoMana);
                break;
        }

        if (accion.objetivoEsElEquipo)
        {
            objetivo = this;
        }

        if (accion.estatico)
        {
            EjecutarEfecto(accion, objetivo);
        }
        else
        {
            Vector3 posInicial = transform.position;
            transform.LookAt(objetivo.transform.position);
            nv.SetDestination(objetivo.transform.position);

            while (Vector3.Distance(transform.position, objetivo.transform.position) > 2f)
            {
                yield return null;
            }

            EjecutarEfecto(accion, objetivo);

            transform.LookAt(posInicial);
            nv.SetDestination(posInicial);

            while (Vector3.Distance(transform.position, posInicial) > 0.1f)
            {
                yield return null;
            }
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
                var efecto = new EfectoActivo
                {
                    nombre = accion.efectoSecundario,
                    tipo = (TipoEfecto)Enum.Parse(typeof(TipoEfecto), accion.efectoSecundario),
                    modificador = accion.argumento,
                    duracionTurnos = 3
                };
                objetivo.efectosActivos.Add(efecto);
                break;
            default:
                Debug.LogWarning($"Acción '{accion.mensaje}' no implementada.");
                break;
        }

        if (!string.IsNullOrEmpty(accion.animacionTrigger))
        {
            anim?.SetTrigger(accion.animacionTrigger);
        }
    }

    public void AplicarEfectosPorTurno()
    {
        for (int i = efectosActivos.Count - 1; i >= 0; i--)
        {
            var efecto = efectosActivos[i];
            efecto.AplicarEfectoPorTurno(this);
            if (!efecto.EsPermanente)
            {
                efecto.duracionTurnos--;
                if (efecto.duracionTurnos <= 0)
                {
                    efectosActivos.RemoveAt(i);
                    Debug.Log($"{nombre} pierde el efecto: {efecto.nombre}");
                }
            }
        }
    }

    public void CambiarVida(int cantidad)
    {
        vida += cantidad;
        Debug.Log($"[{nombre}] vida modificada en {cantidad}, nueva vida: {vida}");

        if (vida <= 0)
        {
            sigueVivo = false;
            Debug.Log($"{nombre} ha sido derrotado.");
        }
    }

    public void CambiarMana(int cantidad)
    {
        mana += cantidad;
        Debug.Log($"[{nombre}] mana modificado en {cantidad}, nuevo mana: {mana}");
    }

    public void CambiarSanidad(int cantidad)
    {
        sanidad += cantidad;
        Debug.Log($"[{nombre}] sanidad modificada en {cantidad}, nueva sanidad: {sanidad}");

        if (sanidad <= 0)
        {
            Debug.Log($"{nombre} ha perdido la cordura.");
            // Lógica adicional si se desea
        }
    }
}
