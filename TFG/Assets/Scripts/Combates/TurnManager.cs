using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    [SerializeField] private List<Luchador> luchadoresEnCombate;
    private Queue<Luchador> ordenTurnos = new();
    private Luchador actual;

    public Luchador Actual => actual;

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

    private void InicializarTurnos()
    {
        ordenTurnos.Clear();
        
        foreach (var luchador in luchadoresEnCombate)
        {
            if (luchador != null && luchador.sigueVivo)
                ordenTurnos.Enqueue(luchador);
        }
    }

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

            if (actual.saltarSiguienteTurno)
            {
                Debug.Log($"{actual.nombre} salta su turno por Grim Fandango.");
                actual.saltarSiguienteTurno = false;
                ordenTurnos.Enqueue(actual);
                yield return null;
                continue;
            }

            SeleccionDeObjetivo.Instance.LimpiarSeleccion();
            actual.AplicarEfectosPorTurno();

            if (actual.Aliado)
            {
                Debug.Log(actual.nombre);
                CombatUI.Instance.MostrarCartas(actual.cartasDisponibles);
                EnemyAIController.Instance.ReiniciarMemoriaDeObjetivos();
                PlayerInputController.Instance.PrepararTurno();
                yield return new WaitUntil(() => PlayerInputController.TurnoFinalizado);
            }
            else
            {
                EnemyAIController.Instance.EjecutarTurno(actual);
                yield return new WaitUntil(() => EnemyAIController.TurnoFinalizado);
            }

            ordenTurnos.Enqueue(actual);
            actual = null;
            yield return null;
        }
    }
}
