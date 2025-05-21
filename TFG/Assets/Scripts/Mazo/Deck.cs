using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestiona el mazo de cartas: robo, descarte, y colocación en anclas visuales.
/// </summary>
public class Deck : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Carta prefabCarta;
    [SerializeField] private Canvas canvasCartas;

    [Header("Referencias externas")]
    [SerializeField] public Mano mano;

    private CardCollection coleccionBase;
    private readonly List<Carta> pilaMazo = new();
    private readonly List<Carta> pilaDescarte = new();
    public List<Carta> CartasEnMano { get; private set; } = new();

    public void ActivarVisual(bool activo)
    {
        if (canvasCartas != null)
            canvasCartas.gameObject.SetActive(activo);
    }

    public void SetCardCollection(CardCollection nuevaColeccion)
    {
        coleccionBase = nuevaColeccion;
        ReiniciarMazo();
    }

    public void ReiniciarMazo()
    {
        mano.LiberarTodasLasPosiciones();

        foreach (var carta in CartasEnMano)
            if (carta != null) DestroyImmediate(carta.gameObject);

        foreach (var carta in pilaMazo)
            if (carta != null) DestroyImmediate(carta.gameObject);

        pilaMazo.Clear();
        pilaDescarte.Clear();
        CartasEnMano.Clear();

        InicializarMazo();
    }

    private void InicializarMazo()
    {
        if (coleccionBase == null || prefabCarta == null || canvasCartas == null)
        {
            Debug.LogWarning("Deck: Falta asignar referencias necesarias.");
            return;
        }

        foreach (var data in coleccionBase.CartasEnLaColeccion)
        {
            if (data == null) continue;

            var carta = Instantiate(prefabCarta, canvasCartas.transform);
            carta.SetUp(data);
            carta.gameObject.SetActive(false);
            pilaMazo.Add(carta);
        }

        BarajarCartas(pilaMazo);
    }

    /// <summary>
    /// Roba X cartas nuevas y las coloca en la mano.
    /// </summary>
    public void RobarCartas(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (pilaMazo.Count == 0 && pilaDescarte.Count > 0)
                MezclarDescarteEnMazo();

            if (pilaMazo.Count == 0)
            {
                Debug.LogWarning("No hay cartas disponibles para robar.");
                return;
            }

            var carta = pilaMazo[0];
            pilaMazo.RemoveAt(0);
            CartasEnMano.Add(carta);

            Transform ancla = mano.ObtenerSiguienteAncla(out int index);
            if (ancla != null)
            {
                carta.transform.SetParent(ancla, false);
                carta.transform.localPosition = Vector3.zero;
                carta.transform.localRotation = Quaternion.identity;
                carta.indiceAncla = index;
            }
            else
            {
                Debug.LogWarning("No hay posiciones disponibles para colocar carta.");
                continue;
            }

            carta.gameObject.SetActive(true);
        }
    }

    public void DescartarCarta(Carta carta)
    {
        if (CartasEnMano.Contains(carta))
        {
            CartasEnMano.Remove(carta);
            pilaDescarte.Add(carta);
            carta.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Roba una nueva carta para la posición especificada (índice).
    /// </summary>
    public void RobarCartaEnPosicion(int index)
    {
        if (pilaMazo.Count == 0 && pilaDescarte.Count > 0)
            MezclarDescarteEnMazo();

        if (pilaMazo.Count == 0)
        {
            Debug.LogWarning("No hay cartas disponibles para robar.");
            return;
        }

        var carta = pilaMazo[0];
        pilaMazo.RemoveAt(0);
        CartasEnMano.Add(carta);

        Transform ancla = mano.ObtenerAnclaEnIndice(index);
        if (ancla != null)
        {
            carta.transform.SetParent(ancla, false);
            carta.transform.localPosition = Vector3.zero;
            carta.transform.localRotation = Quaternion.identity;
            carta.indiceAncla = index;
            carta.gameObject.SetActive(true);
            Debug.Log($"Robada carta colocada en índice: {index}");
        }
        else
        {
            Debug.LogWarning("No se pudo colocar la carta en la posición especificada.");
        }
    }

    private void MezclarDescarteEnMazo()
    {
        pilaMazo.AddRange(pilaDescarte);
        pilaDescarte.Clear();
        BarajarCartas(pilaMazo);
        Debug.Log("Se mezcló el descarte en el mazo.");
    }

    private void BarajarCartas(List<Carta> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lista[i], lista[j]) = (lista[j], lista[i]);
        }
    }

    /// <summary>
    /// Descarta una carta y roba otra en su misma posición.
    /// </summary>
    public void DescartarYReemplazarCarta(Carta carta)
    {
        if (!CartasEnMano.Contains(carta))
        {
            Debug.LogWarning("La carta no está en la mano.");
            return;
        }

        int index = carta.indiceAncla;

        mano.LiberarPosicion(index);

        CartasEnMano.Remove(carta);
        pilaDescarte.Add(carta);
        carta.gameObject.SetActive(false);

        RobarCartaEnPosicion(index);

        Debug.Log($"Carta descartada en índice {index} y reemplazada.");
    }
}
