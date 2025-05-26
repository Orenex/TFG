using UnityEngine;

// Controlador de entrada para el jugador humano
public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get; private set; }
    public static bool TurnoFinalizado { get; private set; }

    private bool enSeleccionDeObjetivo = false;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Prepara el turno activando la interfaz del jugador
    public void PrepararTurno()
    {
        TurnoFinalizado = false;
        CombatUI.Instance.ActivarInterfazJugador(true);
        SeleccionDeObjetivo.Instance.LimpiarSeleccion();
    }

    // Finaliza el turno del jugador
    public static void TerminarTurno()
    {
        TurnoFinalizado = true;
        CombatUI.Instance.ActivarInterfazJugador(false);
        SeleccionDeObjetivo.Instance.LimpiarSeleccion();
    }

    private void Update()
    {
        if (!enSeleccionDeObjetivo)
        {
            // Si presiona ENTER y hay carta seleccionada, pasa a selección de objetivo
            if (Input.GetKeyDown(KeyCode.Return))
            {
                var carta = HandManager.Instance.ObtenerCartaSeleccionada();
                if (carta != null)
                {
                    enSeleccionDeObjetivo = true;
                    Debug.Log("Selecciona un objetivo con A/D, ENTER para confirmar.");
                }
            }
        }
        else
        {
            // Navega entre objetivos con A y D, y ENTER para confirmar
            if (Input.GetKeyDown(KeyCode.A))
                SeleccionDeObjetivo.Instance.CambiarSeleccion(-1);
            else if (Input.GetKeyDown(KeyCode.D))
                SeleccionDeObjetivo.Instance.CambiarSeleccion(1);
            else if (Input.GetKeyDown(KeyCode.Return))
                ConfirmarUso();
        }
    }

    // Confirma el uso de una carta sobre un objetivo
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
}
