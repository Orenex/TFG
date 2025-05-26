using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que gestiona el flujo de turnos entre los luchadores
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [SerializeField] private List<Luchador> luchadoresEnCombate; // Lista de todos los combatientes
    private Queue<Luchador> ordenTurnos = new(); // Cola de turnos a ejecutar
    private Luchador actual;

    public Luchador Actual => actual; // Luchador cuyo turno está activo

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        InicializarTurnos();
        StartCoroutine(GestionarTurnos());
    }

    // Carga los luchadores vivos en la cola de turnos
    private void InicializarTurnos()
    {
        ordenTurnos.Clear();

        foreach (var luchador in luchadoresEnCombate)
        {
            if (luchador != null && luchador.sigueVivo)
                ordenTurnos.Enqueue(luchador);
        }
    }

    // Ciclo principal de ejecución de turnos
    private IEnumerator GestionarTurnos()
    {
        while (true)
        {
            if (ordenTurnos.Count == 0)
            {
                InicializarTurnos();
                yield return null;
            }

            actual = ordenTurnos.Dequeue();

            if (actual == null || !actual.sigueVivo)
                continue;

            // Si el luchador tiene que saltarse su turno (por efectos)
            if (actual.saltarSiguienteTurno)
            {
                Debug.Log($"{actual.nombre} salta su turno por Grim Fandango.");
                actual.saltarSiguienteTurno = false;
                ordenTurnos.Enqueue(actual);
                yield return null;
                continue;
            }

            // Limpia selección de objetivos y aplica efectos activos
            SeleccionDeObjetivo.Instance.LimpiarSeleccion();
            actual.AplicarEfectosPorTurno();

            // Si el luchador es un jugador humano
            if (actual.Aliado)
            {
                Debug.Log(actual.nombre);
                CombatUI.Instance.MostrarCartas(actual.cartasDisponibles);
                EnemyAIController.Instance.ReiniciarMemoriaDeObjetivos();
                PlayerInputController.Instance.PrepararTurno();
                yield return new WaitUntil(() => PlayerInputController.TurnoFinalizado);
            }
            else // Si es un enemigo controlado por la IA
            {
                EnemyAIController.Instance.EjecutarTurno(actual);
                yield return new WaitUntil(() => EnemyAIController.TurnoFinalizado);
            }

            // Añade al final de la cola y continúa el ciclo
            ordenTurnos.Enqueue(actual);
            actual = null;
            yield return null;
        }
    }
}
