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

    // Ejecuta la acci�n de un jugador sobre un objetivo
    public void EjecutarAccionJugador(Accion accion, Luchador objetivo, Accion? accionSecundaria = null)
    {
        var lanzador = TurnManager.Instance.Actual;

        // Validaci�n de recursos y estados
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

        // Efectos negativos autom�ticos
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

        // Inicia la ejecuci�n de la acci�n
        StartCoroutine(ResolverAccion(accion, lanzador, objetivo, true, accionSecundaria));
    }

    // Ejecuta la acci�n de un enemigo IA sobre un objetivo
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

    // Corrutina que resuelve una acci�n de combate
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

    // Verifica si el lanzador tiene suficiente recurso para usar la acci�n
    private bool EsAccionValida(Accion accion, Luchador lanzador)
    {
        return lanzador.sanidad >= accion.costoMana;
    }
}
