using UnityEngine;

// Clase que gestiona la interfaz de usuario durante el combate
public class CombatUI : MonoBehaviour
{
    public static CombatUI Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelCartas;       // Panel donde se muestran las cartas
    [SerializeField] private GameObject botonDescartar;    // Botón para descartar carta
    [SerializeField] private GameObject botonConfirmar;    // Botón para confirmar uso de carta

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Muestra las cartas del jugador activando el panel y preparando la mano
    public void MostrarCartas(CardCollection coleccion)
    {
        Deck.Instance.AsignarColeccion(coleccion);
        HandManager.Instance.PrepararNuevaMano(coleccion);
        panelCartas.SetActive(true);
    }

    // Activa o desactiva la interfaz del jugador en combate
    public void ActivarInterfazJugador(bool activa)
    {
        panelCartas.SetActive(activa);
        botonDescartar.SetActive(activa);
        botonConfirmar.SetActive(activa);
    }

    // Oculta las cartas del jugador
    public void OcultarCartas()
    {
        panelCartas.SetActive(false);
    }
}
