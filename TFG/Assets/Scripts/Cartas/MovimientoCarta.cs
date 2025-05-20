using UnityEngine;
using UnityEngine.EventSystems;

public class MovimientoCarta : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float crecimientoAlSeleccionar = 1.2f;
    private Vector3 escalaOriginal;
    private RectTransform rectTransform;
    private bool seleccionada = false;
    private static MovimientoCarta cartaSeleccionada;

    public Carta CartaData { get; private set; }

    public int indiceAncla { get; set; }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        escalaOriginal = rectTransform.localScale;
        CartaData = GetComponent<Carta>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SeleccionarEstaCarta();
    }

    public void SeleccionarEstaCarta()
    {
        if (cartaSeleccionada != null && cartaSeleccionada != this)
        {
            cartaSeleccionada.Deseleccionar();
        }

        seleccionada = true;
        cartaSeleccionada = this;
        rectTransform.localScale = escalaOriginal * crecimientoAlSeleccionar;
    }

    public void Deseleccionar()
    {
        seleccionada = false;
        rectTransform.localScale = escalaOriginal;
    }

    public static MovimientoCarta ObtenerCartaSeleccionada()
    {
        return cartaSeleccionada;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!seleccionada)
        {
            rectTransform.localScale = escalaOriginal * 1.1f;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!seleccionada)
        {
            rectTransform.localScale = escalaOriginal;
        }
    }
}
