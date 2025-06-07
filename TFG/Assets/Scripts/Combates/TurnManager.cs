using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        InstanciarEnemigosEnPuntos();  //Instanciar enemigos aleatorios al inicio
        InicializarTurnos();           // Carga los luchadores al inicio
        StartCoroutine(GestionarTurnos()); // Inicia el ciclo de turnos
    }

    // NUEVO: Instancia enemigos aleatorios en los puntos de spawn
    private void InstanciarEnemigosEnPuntos()
    {
        bool usarEnemigosGuardados = GameManager.Instance.enemigosActuales != null
                                    && GameManager.Instance.enemigosActuales.Count > 0;

        for (int i = 0; i < puntosDeSpawn.Length; i++)
        {
            GameObject prefab;
            if (usarEnemigosGuardados && i < GameManager.Instance.enemigosActuales.Count)
            {
                string nombre = GameManager.Instance.enemigosActuales[i];
                prefab = prefabsDeEnemigos.FirstOrDefault(p => p.name == nombre);
            }
            else
            {
                prefab = prefabsDeEnemigos[Random.Range(0, prefabsDeEnemigos.Length)];
            }

            GameObject enemigoGO = Instantiate(prefab, puntosDeSpawn[i].position, prefab.transform.rotation);
            Luchador enemigo = enemigoGO.GetComponent<Luchador>();
            if (enemigo != null)
            {
                luchadoresEnCombate.Add(enemigo);
                if (!usarEnemigosGuardados)
                    GameManager.Instance.enemigosActuales.Add(prefab.name); // Guardar enemigos si es primera vez
            }
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
                CombatUI.Instance.MostrarCartas(actual.cartasDisponibles);
                EnemyAIController.Instance.ReiniciarMemoriaDeObjetivos();
                PlayerInputController.Instance.PrepararTurno();

                yield return new WaitUntil(() => PlayerInputController.TurnoFinalizado);

                yield return new WaitForSeconds(2f);
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
