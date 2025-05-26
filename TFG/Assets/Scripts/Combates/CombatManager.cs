using System.Collections;
using UnityEngine;

// Clase que se encarga de ejecutar las acciones de combate, tanto del jugador como de la IA
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Ejecuta la acción de un jugador sobre un objetivo
    public void EjecutarAccionJugador(Accion accion, Luchador objetivo, Accion? accionSecundaria = null)
    {
        var lanzador = TurnManager.Instance.Actual;

        // Validación de recursos y estados
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

        // Efectos negativos automáticos
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

        // Inicia la ejecución de la acción
        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, true, accionSecundaria));
    }

    // Ejecuta la acción de un enemigo IA sobre un objetivo
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

    // Corrutina que resuelve una acción de combate
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

    // Verifica si el lanzador tiene suficiente recurso para usar la acción
    private bool EsAccionValida(Accion accion, Luchador lanzador)
    {
        return lanzador.sanidad >= accion.costoMana;
    }
}
