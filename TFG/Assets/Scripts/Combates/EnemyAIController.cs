using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlador de inteligencia artificial para enemigos en combate
public class EnemyAIController : MonoBehaviour
{
    public static EnemyAIController Instance { get; private set; }
    public static bool TurnoFinalizado { get; set; }

    private Dictionary<Luchador, Luchador> memoriaDeObjetivos = new(); // Memoria para evitar objetivos repetidos

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Ejecuta el turno de un enemigo
    public void EjecutarTurno(Luchador enemigo)
    {
        TurnoFinalizado = false;
        Debug.Log($"Paralizado: {enemigo.estadoEspecial.Paralizado}");

        // Revisa condiciones negativas
        if (enemigo.estadoEspecial.Paralizado)
        {
            Debug.Log($"{enemigo.nombre} está paralizado y pierde su turno.");
            TurnoFinalizado = true;
            return;
        }

        if (enemigo.estadoEspecial.Asqueado)
        {
            Debug.Log($"{enemigo.nombre} está asqueado.");
            enemigo.vida -= 3;
        }
        if (enemigo.estadoEspecial.Sangrado)
        {
            Debug.Log($"{enemigo.nombre} está sangrando.");
            enemigo.vida -= 1;
        }

        // Filtra cartas que el enemigo puede usar con su sanidad actual
        var cartas = enemigo.cartasDisponibles.CartasEnLaColeccion;
        var jugables = cartas.FindAll(c => c != null && enemigo.sanidad >= c.accion.costoMana);

        if (jugables.Count == 0)
        {
            Debug.Log($"{enemigo.nombre} no tiene cartas jugables por falta de sanidad.");
            TurnoFinalizado = true;
            return;
        }

        // Escoge carta y objetivo, y ejecuta acción
        ScriptableCartas carta = ElegirCarta(jugables, enemigo);
        Luchador objetivo = ElegirObjetivo(carta, enemigo);

        if (objetivo != null)
        {
            StartCoroutine(EjecutarAccion(carta.accion, enemigo, objetivo));
        }
        else
        {
            Debug.Log($"{enemigo.nombre} no encontró un objetivo válido.");
            TurnoFinalizado = true;
        }
    }

    // Estrategia básica de selección de carta
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

    // Selección del objetivo para la carta elegida
    private Luchador ElegirObjetivo(ScriptableCartas carta, Luchador lanzador)
    {
        // Si está confundido puede atacar a un aliado enemigo
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

        var posiblesObjetivos = new List<Luchador>(FindObjectsOfType<Luchador>());
        posiblesObjetivos = posiblesObjetivos.FindAll(l => l.sigueVivo && l.Aliado != lanzador.Aliado);

        if (posiblesObjetivos.Count == 0)
        {
            Debug.LogWarning("EnemyAI: No hay enemigos válidos para atacar.");
            return null;
        }

        if (carta.accion.objetivoEsElEquipo)
            return lanzador;

        // Reutiliza objetivo anterior si es válido
        memoriaDeObjetivos.TryGetValue(lanzador, out var ultimoObjetivo);

        if (posiblesObjetivos.Count > 1 && ultimoObjetivo != null && posiblesObjetivos.Contains(ultimoObjetivo))
        {
            posiblesObjetivos.Remove(ultimoObjetivo);
        }

        Luchador objetivoElegido;

        // Elige entre objetivo con menos vida o aleatorio
        if (carta.accion.argumento < 0)
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

    // Ejecuta la acción con una corrutina
    private IEnumerator EjecutarAccion(Accion accion, Luchador lanzador, Luchador objetivo)
    {
        yield return lanzador.EjecutarAccion(accion, objetivo);
        TurnoFinalizado = true;
    }

    // Limpia la memoria para que IA pueda variar objetivos en el siguiente turno
    public void ReiniciarMemoriaDeObjetivos()
    {
        memoriaDeObjetivos.Clear();
    }
}
