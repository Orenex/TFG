using System.Collections.Generic;
using UnityEngine;

public class Mano : MonoBehaviour
{
    [Header("Anclas visuales para cartas")]
    [SerializeField] private Transform[] anclas;
    private bool[] posicionesLibres;

    [Header("Interfaz")]
    [SerializeField] private GameObject botonConfirmar;
    [SerializeField] public Deck deck;

    private List<MovimientoCarta> cartasEnMano = new();
    public static bool IsDiscarding { get; private set; }

    private bool descarteUsadoEsteTurno = false;

    private void Awake()
    {
        posicionesLibres = new bool[anclas.Length];
        for (int i = 0; i < posicionesLibres.Length; i++)
        {
            posicionesLibres[i] = true;
        }
    }

    public void AlternarDescartar()
    {
        IsDiscarding = !IsDiscarding;
        cartasEnMano.Clear();
        MovimientoCarta[] cartas = FindObjectsOfType<MovimientoCarta>();
        cartasEnMano.AddRange(cartas);
    }

    public Transform ObtenerSiguienteAncla(out int index)
    {
        for (int i = 0; i < anclas.Length; i++)
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
        {
            posicionesLibres[index] = true;
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
        if (descarteUsadoEsteTurno)
        {
            Debug.LogWarning("Ya usaste tu descarte este turno.");
            return;
        }

        var seleccionada = MovimientoCarta.ObtenerCartaSeleccionada();
        if (seleccionada == null)
        {
            Debug.LogWarning("No hay carta seleccionada para descartar.");
            return;
        }

        deck.DescartarYReemplazarCarta(seleccionada.CartaData);
        LiberarPosicion(seleccionada.indiceAncla);
        seleccionada.gameObject.SetActive(false);

        descarteUsadoEsteTurno = true;
    }

    public bool NoHayEspacioDisponible()
    {
        return ObtenerSiguienteAncla(out _) == null;
    }

    public void ReiniciarAnclas()
    {
        for (int i = 0; i < posicionesLibres.Length; i++)
        {
            posicionesLibres[i] = true;
        }
    }

    public void LiberarTodasLasPosiciones()
    {
        for (int i = 0; i < posicionesLibres.Length; i++)
        {
            posicionesLibres[i] = true;
        }
    }

    public void ReiniciarDescartesDelTurno()
    {
        descarteUsadoEsteTurno = false;
    }

    public Transform ObtenerAnclaEnIndice(int index)
    {
        if (index >= 0 && index < anclas.Length)
        {
            posicionesLibres[index] = false;
            return anclas[index];
        }

        return null;
    }
}
