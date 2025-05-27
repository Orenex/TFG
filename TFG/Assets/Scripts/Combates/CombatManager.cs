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

    // Ejecuta una acción de un jugador sobre un objetivo
    public void EjecutarAccionJugador(Accion accion, Luchador objetivo, Accion? accionSecundaria = null)
    {
        var lanzador = TurnManager.Instance.Actual;

        // Verifica si tiene suficientes recursos para ejecutar la acción
        if (!EsAccionValida(accion, lanzador))
        {
            Debug.LogWarning("No tienes suficiente sanidad para usar esta acción.");
            return;
        }

        // Si el lanzador está paralizado, pierde el turno
        if (lanzador.estadoEspecial.Paralizado)
        {
            Debug.LogWarning($"{lanzador.nombre} está paralizado y no puede actuar.");
            PlayerInputController.TerminarTurno();
            return;
        }

        // Sangrado hace que pierda vida y no ejecute acción
        if (lanzador.estadoEspecial.Sangrado)
        {
            Debug.LogWarning($"{lanzador.nombre} está sangrando");
            objetivo.CambiarVida(-1);
            return;
        }

        // Asqueado provoca una pérdida mayor de vida
        if (lanzador.estadoEspecial.Asqueado)
        {
            Debug.LogWarning($"{lanzador.nombre} está asqueado");
            objetivo.CambiarVida(-3);
            return;
        }

        // Si todo está bien, se lanza la corrutina para resolver la acción
        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, true, accionSecundaria));
    }

    // Ejecuta una acción por parte de un enemigo IA
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
        // Si el objetivo no es válido, se cancela
        if (objetivo == null || !objetivo.sigueVivo)
        {
            Debug.LogWarning("Objetivo inválido.");
            if (jugador) PlayerInputController.TerminarTurno();
            else EnemyAIController.TurnoFinalizado = true;
            yield break;
        }

        // Ejecuta la acción principal (y secundaria si la hay)
        yield return lanzador.EjecutarAccion(accion, objetivo, accionSecundaria);

        // Marca el fin del turno correspondiente
        if (jugador)
            PlayerInputController.TerminarTurno();
        else
            EnemyAIController.TurnoFinalizado = true;
    }

    // Verifica si el luchador tiene suficiente recurso para usar la acción
    private bool EsAccionValida(Accion accion, Luchador lanzador)
    {
        return lanzador.sanidad >= accion.costoMana;
    }
}
