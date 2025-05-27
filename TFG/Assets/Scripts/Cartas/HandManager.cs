using System.Collections.Generic;
using UnityEngine;

// Clase encargada de gestionar la mano de cartas del jugador
public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private Transform[] anclas; // Puntos de anclaje visual para las cartas
    [SerializeField] private GameObject cartaPrefab; // Prefab visual de la carta

    private bool[] posicionesLibres; // Controla qué posiciones están disponibles
    private List<CardView> cartasEnMano = new(); // Cartas actualmente en mano
    private bool descarteUsadoEsteTurno = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        posicionesLibres = new bool[anclas.Length];
        for (int i = 0; i < posicionesLibres.Length; i++)
            posicionesLibres[i] = true;
    }

    // Prepara una nueva mano a partir de una colección
    public void PrepararNuevaMano(CardCollection coleccion)
    {
        Debug.Log("PreparandoMano");
        LimpiarMano();

        List<ScriptableCartas> lista = new(coleccion.CartasEnLaColeccion);
        lista.Shuffle();

        for (int i = 0; i < Mathf.Min(lista.Count, anclas.Length); i++)
        {
            CrearCartaEnPosicion(lista[i], i);
        }

        descarteUsadoEsteTurno = false;
    }

    // Instancia una carta en la interfaz y la coloca en la mano
    private void CrearCartaEnPosicion(ScriptableCartas data, int index)
    {
        Transform ancla = anclas[index];
        posicionesLibres[index] = false;

        GameObject nuevaCarta = Instantiate(cartaPrefab, ancla);
        nuevaCarta.transform.localPosition = Vector3.zero;

        var cardView = nuevaCarta.GetComponent<CardView>();
        cardView.Configurar(data, index);

        cartasEnMano.Add(cardView);
    }

    // Descarta una carta desde la mano
    public void DescartarCarta(CardView carta)
    {
        if (!cartasEnMano.Contains(carta)) return;

        posicionesLibres[carta.IndiceAncla] = true;
        cartasEnMano.Remove(carta);
        Destroy(carta.gameObject);
    }

    // Permite reemplazar una carta por otra nueva del mazo
    public void ReemplazarCarta(CardView carta)
    {
        if (descarteUsadoEsteTurno)
        {
            Debug.LogWarning("Ya usaste el descarte este turno.");
            return;
        }

        int index = carta.IndiceAncla;
        DescartarCarta(carta);
        Deck.Instance.EnviarADescarte(carta.Data);

        ScriptableCartas nueva = Deck.Instance.ObtenerCartaAleatoria();
        if (nueva != null)
        {
            CrearCartaEnPosicion(nueva, index);
            descarteUsadoEsteTurno = true;
        }
        else
        {
            Debug.LogWarning("No hay cartas disponibles para reemplazar.");
        }
    }

    // Devuelve una posición libre para colocar una nueva carta
    public Transform ObtenerAnclaLibre(out int index)
    {
        for (int i = 0; i < posicionesLibres.Length; i++)
        {
            if (posicionesLibres[i])
            {
                posicionesLibres[i] = false;
                index = i;
                return anclas[i];
            }
        }

        index = -1;
        return null;
    }

    // Libera todas las posiciones ocupadas y elimina cartas de la mano
    public void LiberarTodasLasPosiciones()
    {
        for (int i = 0; i < posicionesLibres.Length; i++)
            posicionesLibres[i] = true;

        foreach (var c in cartasEnMano)
            Destroy(c.gameObject);

        cartasEnMano.Clear();
    }

    // Limpia visualmente la mano actual
    private void LimpiarMano()
    {
        foreach (var carta in cartasEnMano)
        {
            Destroy(carta.gameObject);
        }

        cartasEnMano.Clear();
        for (int i = 0; i < posicionesLibres.Length; i++)
            posicionesLibres[i] = true;
    }

    // Devuelve la carta actualmente seleccionada
    public CardView ObtenerCartaSeleccionada()
    {
        return cartasEnMano.Find(c => c.Seleccionada);
    }

    // Descarta y reemplaza la carta seleccionada si es posible
    public void ConfirmarDescarte()
    {
        var seleccionada = ObtenerCartaSeleccionada();
        if (seleccionada == null)
        {
            Debug.LogWarning("No hay carta seleccionada para descartar.");
            return;
        }

        ReemplazarCarta(seleccionada);
    }
}
