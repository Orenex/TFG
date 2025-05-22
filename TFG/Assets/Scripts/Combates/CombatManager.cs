using System.Collections;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void EjecutarAccionJugador(Accion accion, Luchador objetivo)
    {
        var lanzador = TurnManager.Instance.Actual;

        if (!EsAccionValida(accion, lanzador))
        {
            Debug.LogWarning("No tienes recursos suficientes.");
            return;
        }

        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, true));
    }

    public void EjecutarAccionEnemigo(Accion accion, Luchador lanzador, Luchador objetivo)
    {
        if (!EsAccionValida(accion, lanzador))
        {
            Debug.LogWarning("IA sin recursos.");
            return;
        }

        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, false));
    }

    private IEnumerator ResolverAccion(Accion accion, Luchador lanzador, Luchador objetivo, bool jugador)
    {
        if (objetivo == null || !objetivo.sigueVivo)
        {
            Debug.LogWarning("Objetivo inválido.");
            yield break;
        }

        yield return lanzador.EjecutarAccion(accion, objetivo);

        if (jugador)
            PlayerInputController.TerminarTurno();
        else
            EnemyAIController.TurnoFinalizado = true;
    }

    private bool EsAccionValida(Accion accion, Luchador lanzador)
    {
        return accion.tipoCoste switch
        {
            RecursoCoste.Mana => lanzador.mana >= accion.costoMana,
            RecursoCoste.Sanidad => lanzador.sanidad >= accion.costoMana,
            _ => false
        };
    }
}
