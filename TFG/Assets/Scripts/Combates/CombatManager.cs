using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Clase que se encarga de ejecutar las acciones de combate, tanto del jugador como de la IA
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }
    public string escena;

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
       /* if (lanzador.estadoEspecial.Sangrado)
        {
            Debug.LogWarning($"{lanzador.nombre} está sangrando");
            lanzador.CambiarVida(-1);
        }*/

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
        yield return lanzador.EjecutarAccion(accion, objetivo, accionSecundaria, lanzador);

        // Marca el fin del turno correspondiente
        if (jugador)
            PlayerInputController.TerminarTurno();
        else
            EnemyAIController.TurnoFinalizado = true;

        ComprobarFinCombate();
    }

    // Verifica si el luchador tiene suficiente recurso para usar la acción
    private bool EsAccionValida(Accion accion, Luchador lanzador)
    {
        return lanzador.sanidad >= accion.costoMana;
    }

    // Comprueba si el combate ha terminado (sin enemigos)
    private void ComprobarFinCombate()
    {
        var enemigosVivos = FindObjectsOfType<Luchador>().Where(l => !l.Aliado && l.sigueVivo).ToList();
        var aliadosVivos = FindObjectsOfType<Luchador>().Where(l => l.Aliado && l.sigueVivo).ToList();

        if (enemigosVivos.Count == 0)
        {
            Debug.Log("¡Combate finalizado! Has ganado.");

            // Solo se guarda el estado si los aliados ganan
            var aliados = FindObjectsOfType<Luchador>().Where(l => l.Aliado).ToList();
            EstadoAliados.Instancia.GuardarEstado(aliados);

            StartCoroutine(CambiarEscenaTrasDemora(escena)); // Escena de victoria
        }
        else if (aliadosVivos.Count == 0)
        {
            Debug.Log("¡Todos los aliados han sido derrotados! Game Over.");

            // No se guarda el estado si es Game Over
            StartCoroutine(CambiarEscenaTrasDemora("GameOver")); // Escena de derrota
        }
    }

    private IEnumerator CambiarEscenaTrasDemora(string nombreEscena)
    {
    yield return new WaitForSeconds(2f); // Tiempo para que se vea el final del combate
    SceneManager.LoadScene(nombreEscena);
    }

}
