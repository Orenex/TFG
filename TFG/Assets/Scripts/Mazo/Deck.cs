using System.Collections.Generic;
using UnityEngine;

// Clase encargada de gestionar el mazo de cartas y su descarte.
public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }

    [SerializeField] private CardCollection coleccionBase;

    private List<ScriptableCartas> pilaCartas = new();
    private List<ScriptableCartas> pilaDescarte = new();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        InicializarMazo();
    }

    // Inicializa el mazo con la colección base
    private void InicializarMazo()
    {
        if (coleccionBase == null || coleccionBase.CartasEnLaColeccion.Count == 0)
            return;

        pilaCartas.Clear();
        pilaDescarte.Clear();
        pilaCartas.AddRange(coleccionBase.CartasEnLaColeccion);
        pilaCartas.Shuffle();
    }

    // Devuelve una carta aleatoria del mazo
    public ScriptableCartas ObtenerCartaAleatoria()
    {
        if (pilaCartas.Count == 0)
        {
            if (pilaDescarte.Count > 0)
                MezclarDescarteEnMazo();
            else
                return null;
        }

        var carta = pilaCartas[0];
        pilaCartas.RemoveAt(0);
        return carta;
    }

    // Envía una carta al montón de descarte
    public void EnviarADescarte(ScriptableCartas carta)
    {
        if (carta != null)
            pilaDescarte.Add(carta);
    }

    // Cambia la colección base y reinicia el mazo
    public void AsignarColeccion(CardCollection nueva)
    {
        coleccionBase = nueva;
        InicializarMazo();
    }

    // Mezcla las cartas del descarte nuevamente en el mazo
    private void MezclarDescarteEnMazo()
    {
        pilaCartas.AddRange(pilaDescarte);
        pilaDescarte.Clear();
        pilaCartas.Shuffle();
    }
}
