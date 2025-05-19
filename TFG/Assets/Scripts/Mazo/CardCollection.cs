using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card Collection")]
public class CardCollection : ScriptableObject
{
    [field: SerializeField] public List<ScriptableCartas> CartasEnLaColeccion { get; private set; } = new();

    /// <summary>
    /// Agrega una carta a la colección si no está presente.
    /// </summary>
    public bool AgregarCarta(ScriptableCartas carta)
    {
        if (carta == null)
        {
            Debug.LogWarning("Intentaste agregar una carta nula.");
            return false;
        }

        if (!CartasEnLaColeccion.Contains(carta))
        {
            CartasEnLaColeccion.Add(carta);
            return true;
        }

        Debug.LogWarning($"La carta '{carta.nombreCarta}' ya está en la colección.");
        return false;
    }

    /// <summary>
    /// Quita una carta si existe en la colección.
    /// </summary>
    public bool QuitarCarta(ScriptableCartas carta)
    {
        if (CartasEnLaColeccion.Contains(carta))
        {
            CartasEnLaColeccion.Remove(carta);
            return true;
        }

        Debug.LogWarning("La carta no está en la colección.");
        return false;
    }

    /// <summary>
    /// Crea una copia de esta colección (por si quieres que cada personaje tenga su propia instancia).
    /// </summary>
    public CardCollection ClonarColeccion()
    {
        var clone = ScriptableObject.CreateInstance<CardCollection>();
        clone.CartasEnLaColeccion.AddRange(this.CartasEnLaColeccion);
        return clone;
    }
}
