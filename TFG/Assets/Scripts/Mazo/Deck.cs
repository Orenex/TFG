using System.Collections.Generic;
using UnityEngine;

// Clase encargada de gestionar el mazo de cartas y su descarte
public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }

    [SerializeField] private CardCollection coleccionBase; // Colección inicial de cartas

    private List<ScriptableCartas> pilaCartas = new(); // Cartas disponibles para robar
    private List<ScriptableCartas> pilaDescarte = new(); // Cartas descartadas que pueden reciclarse

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        InicializarMazo();
    }

    // Inicializa el mazo con la colección base y la mezcla
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
                return null; // No hay cartas disponibles
        }

        var carta = pilaCartas[0];
        pilaCartas.RemoveAt(0);
        return carta;
    }

    // Agrega una carta al montón de descarte
    public void EnviarADescarte(ScriptableCartas carta)
    {
        if (carta != null)
            pilaDescarte.Add(carta);
    }

    // Cambia la colección base por otra y reinicia el mazo
    public void AsignarColeccion(CardCollection nueva)
    {
        coleccionBase = nueva;
        InicializarMazo();
    }

    // Mezcla las cartas del descarte y las vuelve a agregar al mazo principal
    private void MezclarDescarteEnMazo()
    {
        pilaCartas.AddRange(pilaDescarte);
        pilaDescarte.Clear();
        pilaCartas.Shuffle();
    }
}
