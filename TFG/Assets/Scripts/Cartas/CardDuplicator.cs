using UnityEngine;

public class CardDuplicator : MonoBehaviour
{
    public static CardDuplicator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void DuplicarCarta(CardView cartaDuplicadora)
    {
        var seleccionada = HandManager.Instance.ObtenerCartaSeleccionada();

        if (seleccionada == null || seleccionada == cartaDuplicadora)
        {
            Debug.LogWarning("Selecciona otra carta válida para duplicar.");
            return;
        }

        if (seleccionada.Data.tipo == TipoCarta.Duplicadora)
        {
            Debug.LogWarning("No puedes duplicar una carta duplicadora.");
            return;
        }

        Transform ancla = HandManager.Instance.ObtenerAnclaLibre(out int index);
        if (ancla == null)
        {
            Debug.LogWarning("No hay espacio para duplicar la carta.");
            return;
        }

        GameObject duplicadoGO = Instantiate(cartaDuplicadora.gameObject, ancla);
        duplicadoGO.transform.localPosition = Vector3.zero;

        var duplicado = duplicadoGO.GetComponent<CardView>();
        duplicado.Configurar(seleccionada.Data, index);
        duplicado.gameObject.SetActive(true);
    }
}
