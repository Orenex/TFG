using UnityEngine;

public class CardActionExecutor : MonoBehaviour
{
    public static CardActionExecutor Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void EjecutarCarta(CardView carta, Luchador objetivo)
    {
        if (carta == null || carta.Data == null)
        {
            Debug.LogWarning("Carta inválida.");
            return;
        }

        var tipo = carta.Data.tipo;

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

        CombatManager.Instance.EjecutarAccionJugador(carta.Data.accion, objetivo);
        FinalizarCarta(carta);
    }

    private void FinalizarCarta(CardView carta)
    {
        HandManager.Instance.DescartarCarta(carta);
        Deck.Instance.EnviarADescarte(carta.Data);
        carta.gameObject.SetActive(false);
    }
}
