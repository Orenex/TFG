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

        var lanzador = FightManager.Instance.GetLuchadorActual();
        var accion = carta.DataCarta.accion;

        bool tieneRecursos = accion.tipoCoste switch
        {
            RecursoCoste.Mana => lanzador.mana >= accion.costoMana,
            RecursoCoste.Sanidad => lanzador.sanidad >= accion.costoMana,
            _ => false
        };

        if (!tieneRecursos)
        {
            Debug.LogWarning("No tienes recursos suficientes para usar esta carta.");
            return;
        }

        CardActionExecutor.Instance.JugarCarta(carta, objetivo);
    }
}
