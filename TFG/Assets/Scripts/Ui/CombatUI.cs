using UnityEngine;

public class CombatUI : MonoBehaviour
{
    public static CombatUI Instance { get; private set; }

    [Header("Referencias UI")]
    [SerializeField] private GameObject panelCartas;
    [SerializeField] private GameObject botonDescartar;
    [SerializeField] private GameObject botonConfirmar;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void MostrarCartas(CardCollection coleccion)
    {
        Deck.Instance.AsignarColeccion(coleccion);
        HandManager.Instance.PrepararNuevaMano(coleccion);
        panelCartas.SetActive(true);
    }

    public void ActivarInterfazJugador(bool activa)
    {
        panelCartas.SetActive(activa);
        botonDescartar.SetActive(activa);
        botonConfirmar.SetActive(activa);
    }

    public void OcultarCartas()
    {
        panelCartas.SetActive(false);
    }
}
