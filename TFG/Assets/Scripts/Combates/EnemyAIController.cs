using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public static EnemyAIController Instance { get; private set; }
    public static bool TurnoFinalizado { get; set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void EjecutarTurno(Luchador enemigo)
    {
        TurnoFinalizado = false;

        var cartas = enemigo.cartasDisponibles.CartasEnLaColeccion;

        var jugables = cartas.FindAll(c =>
            c != null &&
            ((c.accion.tipoCoste == RecursoCoste.Mana && enemigo.mana >= c.accion.costoMana) ||
             (c.accion.tipoCoste == RecursoCoste.Sanidad && enemigo.sanidad >= c.accion.costoMana))
        );

        if (jugables.Count == 0)
        {
            TurnoFinalizado = true;
            return;
        }

        ScriptableCartas carta = ElegirCarta(jugables, enemigo);
        Luchador objetivo = ElegirObjetivo(carta, enemigo);

        if (objetivo != null)
        {
            StartCoroutine(EjecutarAccion(carta.accion, enemigo, objetivo));
        }
        else
        {
            TurnoFinalizado = true;
        }
    }

    private ScriptableCartas ElegirCarta(List<ScriptableCartas> cartas, Luchador lanzador)
    {
        if (lanzador.vida <= 10)
        {
            var curativas = cartas.FindAll(c => c.accion.mensaje == "CambiarVida" && c.accion.argumento > 0);
            if (curativas.Count > 0)
                return curativas[Random.Range(0, curativas.Count)];
        }

        var ofensivas = cartas.FindAll(c => c.accion.mensaje == "CambiarVida" && c.accion.argumento < 0);
        if (ofensivas.Count > 0)
            return ofensivas[Random.Range(0, ofensivas.Count)];

        return cartas[Random.Range(0, cartas.Count)];
    }

    private Luchador ElegirObjetivo(ScriptableCartas carta, Luchador lanzador)
    {
        var posiblesObjetivos = new List<Luchador>(FindObjectsOfType<Luchador>());
        posiblesObjetivos = posiblesObjetivos.FindAll(l => l.sigueVivo && l.Aliado != lanzador.Aliado);

        if (posiblesObjetivos.Count == 0)
        {
            Debug.LogWarning("EnemyAI: No hay enemigos v�lidos para atacar.");
            return null;
        }

        // Si la carta cura o apoya, usa a s� mismo
        if (carta.accion.objetivoEsElEquipo)
            return lanzador;

        // Si la carta hace da�o, buscar al m�s d�bil
        if (carta.accion.argumento < 0)
        {
            posiblesObjetivos.Sort((a, b) => a.vida.CompareTo(b.vida));
            return posiblesObjetivos[0];
        }

        // Por defecto, primer enemigo v�lido
        return posiblesObjetivos[0];
    }


    private IEnumerator EjecutarAccion(Accion accion, Luchador lanzador, Luchador objetivo)
    {
        yield return lanzador.EjecutarAccion(accion, objetivo);
        TurnoFinalizado = true;
    }
}
