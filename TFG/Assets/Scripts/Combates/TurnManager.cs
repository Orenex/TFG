using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase que gestiona el flujo de turnos entre los luchadores en combate
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [SerializeField] private List<Luchador> luchadoresEnCombate; // Todos los luchadores en escena
    [SerializeField] private GameObject[] prefabsDeEnemigos;     // Prefabs de enemigos disponibles
    [SerializeField] private Transform[] puntosDeSpawn;          // Puntos de aparición de enemigos

    private Queue<Luchador> ordenTurnos = new(); // Cola de turnos a ejecutar
    private Luchador actual; // Luchador cuyo turno está activo

    public Luchador Actual => actual; // Propiedad de solo lectura para acceder al luchador actual

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        InstanciarEnemigosEnPuntos();  // NUEVO: Instanciar enemigos aleatorios al inicio
        InicializarTurnos();           // Carga los luchadores al inicio
        StartCoroutine(GestionarTurnos()); // Inicia el ciclo de turnos
    }

    // NUEVO: Instancia enemigos aleatorios en los puntos de spawn
    private void InstanciarEnemigosEnPuntos()
    {
        foreach (Transform punto in puntosDeSpawn)
        {
            // Elige un prefab aleatorio
            GameObject prefab = prefabsDeEnemigos[Random.Range(0, prefabsDeEnemigos.Length)];

            // Instancia el enemigo en el punto con su rotación
            GameObject enemigoGO = Instantiate(prefab, punto.position, prefab.transform.rotation);

            // Obtiene el componente Luchador
            Luchador enemigo = enemigoGO.GetComponent<Luchador>();

            // Añade al listado de luchadores
            if (enemigo != null)
                luchadoresEnCombate.Add(enemigo);
        }
    }

    // Inicializa la cola de turnos con luchadores vivos
    private void InicializarTurnos()
    {
        ordenTurnos.Clear();

        foreach (var luchador in luchadoresEnCombate)
        {
            if (luchador != null && luchador.sigueVivo)
                ordenTurnos.Enqueue(luchador);
        }
    }

    // Corrutina principal que gestiona la secuencia de turnos
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

            // Si el luchador está afectado por GrimFandango, salta su turno
            if (actual.saltarSiguienteTurno)
            {
                Debug.Log($"{actual.nombre} salta su turno por Grim Fandango.");
                actual.saltarSiguienteTurno = false;
                ordenTurnos.Enqueue(actual);
                yield return null;
                continue;
            }

            // Limpia la selección de objetivos previa y aplica efectos por turno
            SeleccionDeObjetivo.Instance.LimpiarSeleccion();
            actual.AplicarEfectosPorTurno();

            // Turno del jugador humano
            if (actual.Aliado)
            {
                Debug.Log(actual.nombre);
                CombatUI.Instance.MostrarCartas(actual.cartasDisponibles);
                EnemyAIController.Instance.ReiniciarMemoriaDeObjetivos();
                PlayerInputController.Instance.PrepararTurno();

                yield return new WaitUntil(() => PlayerInputController.TurnoFinalizado);
            }
            else // Turno de enemigo controlado por IA
            {
                EnemyAIController.Instance.EjecutarTurno(actual);
                yield return new WaitUntil(() => EnemyAIController.TurnoFinalizado);
            }

            // Reencola al luchador y limpia referencia
            ordenTurnos.Enqueue(actual);
            actual = null;
            yield return null;
        }
    }
}
