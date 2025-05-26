using UnityEngine;

// Clase encargada de duplicar cartas seleccionadas por el jugador.
public class CardDuplicator : MonoBehaviour
{
    public static CardDuplicator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Método que realiza la duplicación de una carta seleccionada.
    public void DuplicarCarta(CardView cartaDuplicadora)
    {
        if (cartaDuplicadora == null || cartaDuplicadora.Data == null)
        {
            Debug.LogWarning("Carta duplicadora inválida.");
            return;
        }

        var seleccionada = HandManager.Instance.ObtenerCartaSeleccionada();

        if (seleccionada == null)
        {
            Debug.LogWarning("Debes seleccionar otra carta para duplicar.");
            return;
        }

        if (seleccionada == cartaDuplicadora)
        {
            Debug.LogWarning("No puedes duplicar la carta duplicadora a sí misma.");
            return;
        }

        if (seleccionada.Data.tipo == TipoCarta.Duplicadora)
        {
            Debug.LogWarning("No puedes duplicar una carta duplicadora.");
            return;
        }

        // Busca una posición libre en la mano para colocar la nueva carta.
        Transform ancla = HandManager.Instance.ObtenerAnclaLibre(out int index);
        if (ancla == null)
        {
            Debug.LogWarning("No hay espacio disponible para duplicar la carta.");
            return;
        }

        // Instancia visualmente la nueva carta duplicada.
        GameObject duplicadoGO = Instantiate(cartaDuplicadora.gameObject, ancla);
        duplicadoGO.transform.localPosition = Vector3.zero;

        var duplicado = duplicadoGO.GetComponent<CardView>();
        duplicado.Configurar(seleccionada.Data, index);
        duplicado.gameObject.SetActive(true);

        Debug.Log($"Se ha duplicado la carta: {seleccionada.Data.nombreCarta}");
    }
}
