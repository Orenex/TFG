using UnityEngine;

// Clase encargada de ejecutar el efecto de una carta seleccionada.
public class CardActionExecutor : MonoBehaviour
{
    public static CardActionExecutor Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Ejecuta una carta sobre un objetivo (Luchador).
    public void EjecutarCarta(CardView carta, Luchador objetivo)
    {
        if (carta == null || carta.Data == null)
        {
            Debug.LogWarning("Carta inválida.");
            return;
        }

        var tipo = carta.Data.tipo;

        // Si es una carta duplicadora, invoca el duplicador y finaliza.
        if (tipo == TipoCarta.Duplicadora)
        {
            CardDuplicator.Instance.DuplicarCarta(carta);
            FinalizarCarta(carta);
            return;
        }

        if (objetivo == null || !objetivo.sigueVivo)
        {
            Debug.LogWarning("Objetivo inválido.");
            return;
        }

        // Ejecuta la acción principal y secundaria de la carta.
        CombatManager.Instance.EjecutarAccionJugador(carta.Data.accion, objetivo, carta.Data.accionSecundaria);

        FinalizarCarta(carta);
    }

    // Desactiva visualmente la carta y la envía al descarte.
    private void FinalizarCarta(CardView carta)
    {
        HandManager.Instance.DescartarCarta(carta);
        Deck.Instance.EnviarADescarte(carta.Data);
        carta.gameObject.SetActive(false);
    }
}
