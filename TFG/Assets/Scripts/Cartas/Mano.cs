using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mano : MonoBehaviour
{
    [Header("Anclas visuales para cartas")]
    [SerializeField] private Transform[] anclas;
    private bool[] posicionesLibres;

    [Header("Interfaz")]
    [SerializeField] private GameObject botonConfirmar;
    [SerializeField] private Deck deck;

    private List<MovimientoCarta> cartasEnMano = new();

    public static bool IsDiscarding { get; private set; }

    
    private void Awake()
    {
        posicionesLibres = new bool[anclas.Length];
        for (int i = 0; i < posicionesLibres.Length; i++)
        {
            posicionesLibres[i] = true;
        }

        StartCoroutine(RoboAutomatico());
    }

    private void Update()
    {
        botonConfirmar.SetActive(IsDiscarding);
    }

    public void AlternarDescartar()
    {
        IsDiscarding = !IsDiscarding;
        cartasEnMano.Clear();

        MovimientoCarta[] cartas = FindObjectsOfType<MovimientoCarta>();
        foreach (var c in cartas)
        {
            cartasEnMano.Add(c);
        }
    }

    public Transform ObtenerSiguienteAncla(out int index)
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

    public void LiberarPosicion(int index)
    {
        if (index >= 0 && index < posicionesLibres.Length)
            posicionesLibres[index] = true;
    }

    private IEnumerator RoboAutomatico()
    {
        while (true)
        {
            for (int i = 0; i < posicionesLibres.Length; i++)
            {
                if (posicionesLibres[i])
                {
                    yield return new WaitForSeconds(0.2f);
                    deck.RobarCartas(1);
                }
            }
            yield return null;
        }
    }
    public void ConfirmarJugada()
    {
        var seleccionada = MovimientoCarta.ObtenerCartaSeleccionada();
        if (seleccionada == null)
        {
            Debug.LogWarning("No hay carta seleccionada.");
            return;
        }

        var objetivo = SeleccionDeObjetivo.Instance.ObtenerObjetivoActual();
        if (objetivo == null)
        {
            Debug.LogWarning("Selecciona un objetivo.");
            return;
        }

        CardActionExecutor.Instance.JugarCarta(seleccionada.CartaData, objetivo);
        seleccionada.gameObject.SetActive(false);
    }

    public void ConfirmarDescarte()
    {
        var seleccionada = MovimientoCarta.ObtenerCartaSeleccionada();
        if (seleccionada == null)
        {
            Debug.LogWarning("No hay carta seleccionada para descartar.");
            return;
        }

        deck.DescartarCarta(seleccionada.CartaData);
        seleccionada.gameObject.SetActive(false);
        deck.RobarCartas(1);
    }



}
