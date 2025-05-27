using UnityEngine;

// Clase encargada de ejecutar el efecto de una carta seleccionada durante el combate.
public class CardActionExecutor : MonoBehaviour
{
    // Instancia singleton para acceso global
    public static CardActionExecutor Instance { get; private set; }

    private void Awake()
    {
        // Asegura que solo haya una instancia activa
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Ejecuta el efecto de una carta sobre un objetivo (Luchador)
    public void EjecutarCarta(CardView carta, Luchador objetivo)
    {
        // Validación básica de la carta
        if (carta == null || carta.Data == null)
        {
            Debug.LogWarning("Carta inválida.");
            return;
        }

        var tipo = carta.Data.tipo;

        // Si la carta es de tipo Duplicadora, se maneja de forma especial
        if (tipo == TipoCarta.Duplicadora)
        {
            // Se invoca la duplicación de carta y se termina el uso
            CardDuplicator.Instance.DuplicarCarta(carta);
            FinalizarCarta(carta);
            return;
        }

        // Verifica si el objetivo es válido y está vivo
        if (objetivo == null || !objetivo.sigueVivo)
        {
            Debug.LogWarning("Objetivo inválido.");
            return;
        }

        // Llama al CombatManager para ejecutar la acción principal y secundaria de la carta
        CombatManager.Instance.EjecutarAccionJugador(carta.Data.accion, objetivo, carta.Data.accionSecundaria);

        // Finaliza el uso de la carta
        FinalizarCarta(carta);
    }

    // Método auxiliar que descarta visualmente y lógicamente la carta usada
    private void FinalizarCarta(CardView carta)
    {
        // Elimina la carta de la mano
        HandManager.Instance.DescartarCarta(carta);

        // Envía los datos de la carta al mazo de descarte
        Deck.Instance.EnviarADescarte(carta.Data);

        // Desactiva visualmente la carta en la interfaz
        carta.gameObject.SetActive(false);
    }
}
