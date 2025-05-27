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
        // Validaci�n b�sica de la carta
        if (carta == null || carta.Data == null)
        {
            Debug.LogWarning("Carta inv�lida.");
            return;
        }

        var tipo = carta.Data.tipo;

        // Si la carta es de tipo Duplicadora, se maneja de forma especial
        if (tipo == TipoCarta.Duplicadora)
        {
            // Se invoca la duplicaci�n de carta y se termina el uso
            CardDuplicator.Instance.DuplicarCarta(carta);
            FinalizarCarta(carta);
            return;
        }

        // Verifica si el objetivo es v�lido y est� vivo
        if (objetivo == null || !objetivo.sigueVivo)
        {
            Debug.LogWarning("Objetivo inv�lido.");
            return;
        }

        // Llama al CombatManager para ejecutar la acci�n principal y secundaria de la carta
        CombatManager.Instance.EjecutarAccionJugador(carta.Data.accion, objetivo, carta.Data.accionSecundaria);

        // Finaliza el uso de la carta
        FinalizarCarta(carta);
    }

    // M�todo auxiliar que descarta visualmente y l�gicamente la carta usada
    private void FinalizarCarta(CardView carta)
    {
        // Elimina la carta de la mano
        HandManager.Instance.DescartarCarta(carta);

        // Env�a los datos de la carta al mazo de descarte
        Deck.Instance.EnviarADescarte(carta.Data);

        // Desactiva visualmente la carta en la interfaz
        carta.gameObject.SetActive(false);
    }
}
