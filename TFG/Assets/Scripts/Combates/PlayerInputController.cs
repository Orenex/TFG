using UnityEngine;

// Controlador de entrada para el jugador humano
public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get; private set; }
    public static bool TurnoFinalizado { get; private set; } // Indica si el jugador ya hizo su jugada

    private bool enSeleccionDeObjetivo = false; // Determina si está eligiendo un objetivo actualmente

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Prepara el turno del jugador humano
    public void PrepararTurno()
    {
        TurnoFinalizado = false;
        CombatUI.Instance.ActivarInterfazJugador(true);
        SeleccionDeObjetivo.Instance.LimpiarSeleccion();
    }

    // Finaliza el turno del jugador humano
    public static void TerminarTurno()
    {
        TurnoFinalizado = true;
        CombatUI.Instance.ActivarInterfazJugador(false);
        SeleccionDeObjetivo.Instance.LimpiarSeleccion();
    }

    private void Update()
    {
        if (!enSeleccionDeObjetivo)
            return;

        if (Input.GetKeyDown(KeyCode.A))
            SeleccionDeObjetivo.Instance.CambiarSeleccion(-1);
        else if (Input.GetKeyDown(KeyCode.D))
            SeleccionDeObjetivo.Instance.CambiarSeleccion(1);
        else if (Input.GetKeyDown(KeyCode.Return))
            ConfirmarUso();
    }


    // Confirma el uso de una carta sobre el objetivo seleccionado
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
        enSeleccionDeObjetivo = false;
    }

    public void ActivarModoSeleccion()
    {
        enSeleccionDeObjetivo = true;
    }

}
