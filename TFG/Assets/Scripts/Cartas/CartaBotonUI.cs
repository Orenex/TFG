using UnityEngine;
using UnityEngine.UI;

public class CartaBotonUI : MonoBehaviour
{
    private Carta carta;

    [SerializeField] private Button botonJugar;

    private void Awake()
    {
        carta = GetComponent<Carta>();
        botonJugar.onClick.AddListener(JugarCarta);
    }

    private void JugarCarta()
    {
        var objetivo = SeleccionDeObjetivo.Instance.ObtenerObjetivoActual();
        if (objetivo == null)
        {
            Debug.LogWarning("Selecciona un objetivo antes de usar la carta.");
            return;
        }

        CardActionExecutor.Instance.JugarCarta(carta, objetivo);
    }
}
