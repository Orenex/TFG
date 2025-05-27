using UnityEngine;

// Clase encargada de duplicar cartas seleccionadas por el jugador
public class CardDuplicator : MonoBehaviour
{
    public static CardDuplicator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // M�todo que realiza la duplicaci�n de una carta seleccionada
    public void DuplicarCarta(CardView cartaDuplicadora)
    {
        // Validaci�n de la carta duplicadora
        if (cartaDuplicadora == null || cartaDuplicadora.Data == null)
        {
            Debug.LogWarning("Carta duplicadora inv�lida.");
            return;
        }

        // Obtiene la carta seleccionada por el jugador
        var seleccionada = HandManager.Instance.ObtenerCartaSeleccionada();

        if (seleccionada == null)
        {
            Debug.LogWarning("Debes seleccionar otra carta para duplicar.");
            return;
        }

        // No se permite duplicar la propia carta duplicadora
        if (seleccionada == cartaDuplicadora)
        {
            Debug.LogWarning("No puedes duplicar la carta duplicadora a s� misma.");
            return;
        }

        // No se permite duplicar otra carta duplicadora
        if (seleccionada.Data.tipo == TipoCarta.Duplicadora)
        {
            Debug.LogWarning("No puedes duplicar una carta duplicadora.");
            return;
        }

        // Busca una posici�n libre en la mano del jugador
        Transform ancla = HandManager.Instance.ObtenerAnclaLibre(out int index);
        if (ancla == null)
        {
            Debug.LogWarning("No hay espacio disponible para duplicar la carta.");
            return;
        }

        // Crea visualmente la nueva carta duplicada en la interfaz
        GameObject duplicadoGO = Instantiate(cartaDuplicadora.gameObject, ancla);
        duplicadoGO.transform.localPosition = Vector3.zero;

        // Configura la nueva carta con los datos de la carta seleccionada
        var duplicado = duplicadoGO.GetComponent<CardView>();
        duplicado.Configurar(seleccionada.Data, index);
        duplicado.gameObject.SetActive(true);

        Debug.Log($"Se ha duplicado la carta: {seleccionada.Data.nombreCarta}");
    }
}
