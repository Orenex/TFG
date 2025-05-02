using System.Collections;
using System.Collections.Generic; //este using y el anterior para poder utilizar listas, sin �l no funcionar�.
using UnityEngine;



[CreateAssetMenu(menuName = "Card Collection")]

public class CardCollection : ScriptableObject
{
 
    [field:SerializeField] public List<ScriptableCartas> CartasEnLaColeccion { get; private set; }

    //para a�adir y quitar cartas a el mazo:

    public void QuitarCartaDeLaColecci�n(ScriptableCartas carta)
    {
        if (CartasEnLaColeccion.Contains(carta))
        {
            CartasEnLaColeccion.Remove(carta);
        }
        else
        {
            Debug.LogWarning("Datos de la carta no presentes en la colecci�n. Imposible quitar");
        }
    }


    public void AgregarCartaALaColeccion(ScriptableCartas carta)
    {
        CartasEnLaColeccion.Add(carta);
    }
}
