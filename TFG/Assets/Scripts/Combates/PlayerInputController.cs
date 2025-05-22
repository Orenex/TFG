using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get; private set; }
    public static bool TurnoFinalizado { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void PrepararTurno()
    {
        TurnoFinalizado = false;
        CombatUI.Instance.ActivarInterfazJugador(true);
        SeleccionDeObjetivo.Instance.LimpiarSeleccion();
    }

    public static void TerminarTurno()
    {
        TurnoFinalizado = true;
        CombatUI.Instance.ActivarInterfazJugador(false);
        SeleccionDeObjetivo.Instance.LimpiarSeleccion();
    }
    public void ConfirmarUso()
    {
        var carta = HandManager.Instance.ObtenerCartaSeleccionada();
        var objetivo = SeleccionDeObjetivo.Instance.ObtenerObjetivoActual();

        if (carta == null)
        {
            Debug.LogWarning("Selecciona una carta antes de confirmar.");
            return;
        }

        if (objetivo == null)
        {
            Debug.LogWarning("Selecciona un objetivo antes de confirmar.");
            return;
        }

        CardActionExecutor.Instance.EjecutarCarta(carta, objetivo);
    }

}
