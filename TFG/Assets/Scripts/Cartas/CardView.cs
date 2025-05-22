using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private Image imagenCarta;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoDescripcion;
    [SerializeField] private TextMeshProUGUI textoEfecto;
    [SerializeField] private TextMeshProUGUI textoCoste;
    [SerializeField] private TextMeshProUGUI textoTipo;
    [SerializeField] private TextMeshProUGUI textoRecurso;

    [Header("Animación")]
    [SerializeField] private float escalaHover = 1.1f;
    [SerializeField] private float escalaSeleccion = 1.2f;

    public ScriptableCartas Data { get; private set; }
    public int IndiceAncla { get; private set; }
    public bool Seleccionada { get; private set; }

    private RectTransform rect;
    private Vector3 escalaOriginal;

    private static CardView cartaSeleccionada;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        escalaOriginal = rect.localScale;
    }

    public void Configurar(ScriptableCartas data, int indice)
    {
        Data = data;
        IndiceAncla = indice;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (Data == null) return;

        textoNombre.text = Data.nombreCarta;
        textoDescripcion.text = Data.descripcion;
        textoEfecto.text = Data.efecto;
        textoCoste.text = Data.coste.ToString();
        textoTipo.text = Data.tipo.ToString();
        textoRecurso.text = Data.tipoCoste.ToString();
        imagenCarta.sprite = Data.imagen;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Seleccionar();
    }

    public void Seleccionar()
    {
        if (cartaSeleccionada != null && cartaSeleccionada != this)
            cartaSeleccionada.Deseleccionar();

        Seleccionada = true;
        cartaSeleccionada = this;
        rect.localScale = escalaOriginal * escalaSeleccion;
    }

    public void Deseleccionar()
    {
        Seleccionada = false;
        rect.localScale = escalaOriginal;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Seleccionada)
            rect.localScale = escalaOriginal * escalaHover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Seleccionada)
            rect.localScale = escalaOriginal;
    }

    public static CardView ObtenerSeleccionada() => cartaSeleccionada;
}
