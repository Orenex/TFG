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

    public void EjecutarAccionJugador(Accion accion, Luchador objetivo, Accion? accionSecundaria = null)
    {
        var lanzador = TurnManager.Instance.Actual;

        if (!EsAccionValida(accion, lanzador))
        {
            Debug.LogWarning("No tienes suficiente sanidad para usar esta acción.");
            return;
        }

        if (lanzador.estadoEspecial.Paralizado)
        {
            Debug.LogWarning($"{lanzador.nombre} está paralizado y no puede actuar.");
            PlayerInputController.TerminarTurno();
            return;
        }

        if (lanzador.estadoEspecial.Sangrado)
        {
            Debug.LogWarning($"{lanzador.nombre} está sangrando");
            objetivo.CambiarVida(-1); // Daño leve
            return;
        }

        if (lanzador.estadoEspecial.Asqueado)
        {
            Debug.LogWarning($"{lanzador.nombre} está asqueado");
            objetivo.CambiarVida(-3); // Daño leve
            return;
        }

        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, true, accionSecundaria));
    }


    public void EjecutarAccionEnemigo(Accion accion, Luchador lanzador, Luchador objetivo)
    {
        if (!EsAccionValida(accion, lanzador))
        {
            Debug.LogWarning("IA sin recursos de sanidad.");
            EnemyAIController.TurnoFinalizado = true;
            return;
        }

        if (lanzador.estadoEspecial.Paralizado)
        {
            Debug.LogWarning($"{lanzador.nombre} está paralizado y pierde su turno.");
            EnemyAIController.TurnoFinalizado = true;
            return;
        }

        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, false));
    }

    private IEnumerator ResolverAccion(Accion accion, Luchador lanzador, Luchador objetivo, bool jugador, Accion? accionSecundaria = null)
    {
        if (objetivo == null || !objetivo.sigueVivo)
        {
            Debug.LogWarning("Objetivo inválido.");
            if (jugador) PlayerInputController.TerminarTurno();
            else EnemyAIController.TurnoFinalizado = true;
            yield break;
        }

        yield return lanzador.EjecutarAccion(accion, objetivo, accionSecundaria);

        if (jugador)
            PlayerInputController.TerminarTurno();
        else
            EnemyAIController.TurnoFinalizado = true;
    }


    private bool EsAccionValida(Accion accion, Luchador lanzador)
    {
        return lanzador.sanidad >= accion.costoMana;
    }
}
