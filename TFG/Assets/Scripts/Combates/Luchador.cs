using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct Accion
{
    #region Fields and Properties
    public string nombre;
    public bool estatico;
    public bool objetivoEsElEquipo;

    public string mensaje;
    public int argumento;

    public string animacionTrigger;

    public string efectoSecundario;

    public int costoMana;

    #endregion
}

public class Luchador : MonoBehaviour
{
    #region Fields and Properties
    public List<Accion> Acciones;

    public string nombre;
    public int vida;
    public int mana;
    public bool aliado;
    public bool sigueVivo = true;

    Animator anim;
    NavMeshAgent nv;
    #endregion


    #region Methods
    void CambiarVida(int cant)
    {
        vida += cant;
    }
    void CambiarMana(int cant)
    {
        mana += cant;
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        nv = GetComponent<NavMeshAgent>();

        nv.updateRotation = false;
    }

    public IEnumerator EjecutarAccion(Accion accion, Transform objetivo)
    {
        CambiarMana(-accion.costoMana);
        if (accion.objetivoEsElEquipo)
        {
            objetivo = transform;
        }
        if (accion.estatico)
        {
            anim.SetTrigger(accion.animacionTrigger);
            objetivo.SendMessage(accion.mensaje, accion.argumento);
        }
        else
        {
            //si la accion no es estatica vamos a por el objetivo
            Vector3 posInicial = transform.position;

            transform.LookAt(objetivo.transform.position);
            nv.SetDestination(objetivo.position);
            anim.SetFloat("Speed", 1);

            while (Vector3.Distance(transform.position, objetivo.position) > 2)
            {
                yield return null;
            }
            anim.SetFloat("Speed", 0);

            anim.SetTrigger(accion.animacionTrigger);
            objetivo.SendMessage(accion.mensaje, accion.argumento);

            //y volvemos a la posicion inicial
            transform.LookAt(posInicial);
            nv.SetDestination(posInicial);
            anim.SetFloat("Speed", 1);

            while (Vector3.Distance(transform.position, posInicial) > 0.1f)
            {
                yield return null;
            }
            anim.SetFloat("Speed", 0);
        }
    }
    #endregion
}
