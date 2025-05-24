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
            Debug.LogWarning("No tienes suficiente sanidad para usar esta acci�n.");
            return;
        }

        if (lanzador.estadoEspecial.Paralizado)
        {
            Debug.LogWarning($"{lanzador.nombre} est� paralizado y no puede actuar.");
            PlayerInputController.TerminarTurno();
            return;
        }

        if (lanzador.estadoEspecial.Sangrado)
        {
            Debug.LogWarning($"{lanzador.nombre} est� sangrando");
            objetivo.CambiarVida(-1); // Da�o leve
            return;
        }

        if (lanzador.estadoEspecial.Asqueado)
        {
            Debug.LogWarning($"{lanzador.nombre} est� asqueado");
            objetivo.CambiarVida(-3); // Da�o leve
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
            Debug.LogWarning($"{lanzador.nombre} est� paralizado y pierde su turno.");
            EnemyAIController.TurnoFinalizado = true;
            return;
        }

        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, false));
    }

    private IEnumerator ResolverAccion(Accion accion, Luchador lanzador, Luchador objetivo, bool jugador, Accion? accionSecundaria = null)
    {
        if (objetivo == null || !objetivo.sigueVivo)
        {
            Debug.LogWarning("Objetivo inv�lido.");
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
