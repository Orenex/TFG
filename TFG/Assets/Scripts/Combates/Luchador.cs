using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct Accion
{
    public string nombre;
    public bool estatico;
    public bool objetivoEsElEquipo;

    public string mensaje; // debe corresponder a un método del Luchador (ej: "CambiarVida")
    public int argumento;

    public string animacionTrigger;

    public string efectoSecundario;
    public int costoMana;

    internal void Aplicar(Luchador luchadorActual, Luchador aliado)
    {
        throw new NotImplementedException();
    }
}

public class Luchador : MonoBehaviour
{
    #region Estado

    public string nombre;
    public int vida;
    public int mana;
    public bool Aliado;
    public bool sigueVivo = true;

    public CardCollection cartasDisponibles;

    [Header("Acciones disponibles")]
    public List<Accion> Acciones;

    private Animator anim;
    public NavMeshAgent nv;

    #endregion

    #region Unity Methods

    private void Start()
    {
        anim = GetComponent<Animator>();
        nv = GetComponent<NavMeshAgent>();
        nv.updateRotation = false;
    }

    #endregion

    #region Acciones Públicas

    public void EjecutarAccionDesdeCarta(Accion accion, Luchador objetivo)
    {
        StartCoroutine(EjecutarAccion(accion, objetivo));
    }

    public IEnumerator EjecutarAccion(Accion accion, Luchador objetivo)
    {
        Debug.Log($"[{nombre}] ejecuta {accion.nombre} sobre {objetivo.nombre}");

        mana -= accion.costoMana;

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

    #endregion

    #region Métodos Privados

    private void EjecutarEfecto(Accion accion, Luchador objetivo)
    {
        // Aquí puedes mapear los mensajes de forma explícita
        switch (accion.mensaje)
        {
            case "CambiarVida":
                objetivo.CambiarVida(accion.argumento);
                break;
            case "CambiarMana":
                objetivo.CambiarMana(accion.argumento);
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

    private void CambiarVida(int cantidad)
    {
        vida += cantidad;
        Debug.Log($"[{nombre}] vida modificada en {cantidad}, nueva vida: {vida}");

        if (vida <= 0)
        {
            sigueVivo = false;
            Debug.Log($"{nombre} ha sido derrotado.");
        }
    }

    private void CambiarMana(int cantidad)
    {
        mana += cantidad;
        Debug.Log($"[{nombre}] mana modificado en {cantidad}, nuevo mana: {mana}");
    }



    #endregion
}
