using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    public static EnemyAIController Instance { get; private set; }
    public static bool TurnoFinalizado { get; set; }

    // Memoria de objetivos para evitar repeticiones
    private Dictionary<Luchador, Luchador> memoriaDeObjetivos = new();

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
        // Si enemigo está confundido, atacar a otro enemigo al azar
        if (lanzador.estadoEspecial.Confusion)
        {
            var posibles = new List<Luchador>(FindObjectsOfType<Luchador>());
            posibles = posibles.FindAll(l => !l.Aliado && l != lanzador && l.sigueVivo);

            if (posibles.Count > 0)
            {
                var confundido = posibles[Random.Range(0, posibles.Count)];
                Debug.Log($"{lanzador.nombre} está confundido y ataca a {confundido.nombre}");
                lanzador.estadoEspecial.Confusion = false;
                return confundido;
            }
        }

        // Objetivos válidos: aliados del jugador
        var posiblesObjetivos = new List<Luchador>(FindObjectsOfType<Luchador>());
        posiblesObjetivos = posiblesObjetivos.FindAll(l => l.sigueVivo && l.Aliado != lanzador.Aliado);

        if (posiblesObjetivos.Count == 0)
        {
            Debug.LogWarning("EnemyAI: No hay enemigos válidos para atacar.");
            return null;
        }

        if (carta.accion.objetivoEsElEquipo)
            return lanzador;

        memoriaDeObjetivos.TryGetValue(lanzador, out var ultimoObjetivo);

        if (posiblesObjetivos.Count > 1 && ultimoObjetivo != null && posiblesObjetivos.Contains(ultimoObjetivo))
        {
            posiblesObjetivos.Remove(ultimoObjetivo);
        }

        Luchador objetivoElegido;

        if (carta.accion.argumento < 0) // daño
        {
            if (Random.value < 0.5f)
            {
                posiblesObjetivos.Sort((a, b) => a.vida.CompareTo(b.vida));
                objetivoElegido = posiblesObjetivos[0];
            }
            else
            {
                objetivoElegido = posiblesObjetivos[Random.Range(0, posiblesObjetivos.Count)];
            }
        }
        else
        {
            objetivoElegido = posiblesObjetivos[Random.Range(0, posiblesObjetivos.Count)];
        }

        memoriaDeObjetivos[lanzador] = objetivoElegido;
        return objetivoElegido;
    }

    private IEnumerator EjecutarAccion(Accion accion, Luchador lanzador, Luchador objetivo)
    {
        yield return lanzador.EjecutarAccion(accion, objetivo);
        TurnoFinalizado = true;
    }

    public void ReiniciarMemoriaDeObjetivos()
    {
        memoriaDeObjetivos.Clear();
    }
}
