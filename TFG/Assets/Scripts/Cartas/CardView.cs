using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Clase encargada de representar visualmente una carta y gestionar su interacción.
public class CardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    [SerializeField] private Image imagenCarta;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoDescripcion;
    [SerializeField] private TextMeshProUGUI textoEfecto;
    [SerializeField] private TextMeshProUGUI textoCoste;
    [SerializeField] private TextMeshProUGUI textoTipo;

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

    // Configura los datos de la carta y actualiza la UI.
    public void Configurar(ScriptableCartas data, int indice)
    {
        Data = data;
        IndiceAncla = indice;
        ActualizarUI();
    }

    // Refresca el contenido visual de la carta.
    private void ActualizarUI()
    {
        if (Data == null) return;

        textoNombre.text = Data.nombreCarta;
        textoDescripcion.text = Data.descripcion;
        textoEfecto.text = Data.efecto;
        textoCoste.text = Data.costoSanidad.ToString();
        textoTipo.text = Data.tipo.ToString();
        imagenCarta.sprite = Data.imagen;
    }

    // Evento al hacer clic en la carta.
    public void OnPointerClick(PointerEventData eventData)
    {
        Seleccionar();
    }

    // Marca esta carta como seleccionada.
    public void Seleccionar()
    {
        if (cartaSeleccionada != null && cartaSeleccionada != this)
            cartaSeleccionada.Deseleccionar();

        Seleccionada = true;
        cartaSeleccionada = this;
        rect.localScale = escalaOriginal * escalaSeleccion;

        // Iniciar selección de objetivo al seleccionar esta carta
        if (Data != null)
        {
            SeleccionDeObjetivo.Instance.PrepararSeleccion(Data.accion.tipoObjetivo);
            PlayerInputController.Instance.ActivarModoSeleccion();
            Debug.Log("Selecciona un objetivo con A/D, ENTER para confirmar.");
        }
    }


    // Quita la selección visual de la carta.
    public void Deseleccionar()
    {
        Seleccionada = false;
        rect.localScale = escalaOriginal;
    }

    // Evento al pasar el cursor por encima.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Seleccionada)
            rect.localScale = escalaOriginal * escalaHover;
    }

    // Evento al sacar el cursor de la carta.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Seleccionada)
            rect.localScale = escalaOriginal;
    }

    // Devuelve la carta actualmente seleccionada.
    public static CardView ObtenerSeleccionada() => cartaSeleccionada;
}
