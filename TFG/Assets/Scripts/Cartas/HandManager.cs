using System.Collections.Generic;
using UnityEngine;

// Clase encargada de gestionar la mano de cartas del jugador.
public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    [Header("Configuración")]
    [SerializeField] private Transform[] anclas; // Puntos donde se colocan las cartas en pantalla
    [SerializeField] private GameObject cartaPrefab;

    private bool[] posicionesLibres;
    private List<CardView> cartasEnMano = new();
    private bool descarteUsadoEsteTurno = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        posicionesLibres = new bool[anclas.Length];
        for (int i = 0; i < posicionesLibres.Length; i++)
            posicionesLibres[i] = true;
    }

    // Prepara una nueva mano de cartas a partir de una colección
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

    // Instancia y configura una carta en una posición específica
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

    // Elimina una carta de la mano
    public void DescartarCarta(CardView carta)
    {
        if (!cartasEnMano.Contains(carta)) return;

        posicionesLibres[carta.IndiceAncla] = true;
        cartasEnMano.Remove(carta);
        Destroy(carta.gameObject);
    }

    // Reemplaza una carta por otra del mazo, solo una vez por turno
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

    // Devuelve la primera posición libre disponible para una nueva carta
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

    // Libera todas las posiciones y elimina todas las cartas actuales
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

    // Obtiene la carta actualmente seleccionada
    public CardView ObtenerCartaSeleccionada()
    {
        return cartasEnMano.Find(c => c.Seleccionada);
    }

    // Confirma el descarte y reemplazo de la carta seleccionada
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
