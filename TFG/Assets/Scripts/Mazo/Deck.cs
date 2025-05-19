using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestiona el mazo de cartas: robo, descarte, y colocación en anclas visuales.
/// </summary>
public class Deck : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private CardCollection coleccionBase;
    [SerializeField] private Carta prefabCarta;
    [SerializeField] private Canvas canvasCartas;

    [Header("Referencias externas")]
    [SerializeField] private Mano mano;

    private readonly List<Carta> pilaMazo = new();
    private readonly List<Carta> pilaDescarte = new();
    public List<Carta> CartasEnMano { get; private set; } = new();

    private void Start()
    {
        InicializarMazo();
    }

    public void RobarCartas(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (pilaMazo.Count == 0)
            {
                if (pilaDescarte.Count > 0)
                {
                    MezclarDescarteEnMazo();
                }

                if (pilaMazo.Count == 0)
                {
                    Debug.LogWarning("No hay cartas disponibles para robar.");
                    return;
                }
            }

            var carta = pilaMazo[0];
            pilaMazo.RemoveAt(0);
            CartasEnMano.Add(carta);

            // Asignar solo si hay ancla
            Transform ancla = mano.ObtenerSiguienteAncla(out int index);
            if (ancla != null)
            {
                carta.transform.SetParent(ancla, false);
                carta.transform.localPosition = Vector3.zero;
                carta.transform.localRotation = Quaternion.identity;

                Debug.Log(ancla);
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

    private void InicializarMazo()
    {
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
    public void SetCardCollection(CardCollection nuevaColeccion)
    {
        coleccionBase = nuevaColeccion;
        ReiniciarMazo();
    }

    public void ReiniciarMazo()
    {
        // Elimina cartas existentes
        foreach (var carta in pilaMazo)
            Destroy(carta.gameObject);

        pilaMazo.Clear();
        pilaDescarte.Clear();
        CartasEnMano.Clear();

        InicializarMazo();
    }
    public void ActivarVisual(bool activo)
    {
        if (canvasCartas != null)
            canvasCartas.gameObject.SetActive(activo);
    }

}
