using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Controlador de inteligencia artificial para enemigos durante el combate
public class EnemyAIController : MonoBehaviour
{
    public static EnemyAIController Instance { get; private set; }
    public static bool TurnoFinalizado { get; set; }

    private Dictionary<Luchador, Luchador> memoriaDeObjetivos = new(); // Guarda el objetivo anterior de cada enemigo

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    // Ejecuta el turno del enemigo
    public void EjecutarTurno(Luchador enemigo)
    {
        TurnoFinalizado = false;
        Debug.Log($"Paralizado: {enemigo.estadoEspecial.Paralizado}");

        // Condiciones negativas que afectan el turno
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

        // Filtra cartas que puede usar con su sanidad actual
        var cartas = enemigo.cartasDisponibles.CartasEnLaColeccion;
        var jugables = cartas.FindAll(c => c != null && enemigo.sanidad >= c.accion.costoMana);

        if (jugables.Count == 0)
        {
            Debug.Log($"{enemigo.nombre} no tiene cartas jugables por falta de sanidad.");
            TurnoFinalizado = true;
            return;
        }

        // Elige carta y objetivo
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

    private ScriptableCartas ElegirCarta(List<ScriptableCartas> cartas, Luchador lanzador)
    {
        var enemigos = lanzador.ObtenerEnemigosCercanos();
        float vidaRatio = (float)lanzador.vida / lanzador.vidaMaxima;

        // 1. Curarse si está bajo de vida (35% o menos)
        if (vidaRatio <= 0.35f)
        {
            var curacion = cartas.FindAll(c =>
                c.accion.mensaje == "CambiarVida" && c.accion.argumento > 0);

            if (curacion.Count > 0)
                return curacion[Random.Range(0, curacion.Count)];
        }

        // 2. Usar buff si no está activo aún
        var buffs = cartas.FindAll(c =>
            c.tipo == TipoCarta.Buff &&
            !lanzador.efectosActivos.Exists(e => e.nombre == c.accion.efectoSecundario));

        if (buffs.Count > 0 && Random.value < 0.5f)
            return buffs[Random.Range(0, buffs.Count)];

        // 3. Usar debuffs si hay enemigos sin el efecto
        var debuffs = cartas.FindAll(c =>
        {
            if (c.tipo != TipoCarta.Debuff)
                return false;

            if (!System.Enum.TryParse<TipoEfecto>(c.accion.efectoSecundario, out TipoEfecto tipoEfecto))
                return false;

            // Evita lanzar si *alguien ya está paralizado*
            if (tipoEfecto == TipoEfecto.Paralizado &&
                enemigos.Any(e => e.efectosActivos.Exists(ef => ef.tipo == TipoEfecto.Paralizado)))
            {
                return false;
            }

            // Solo enemigos que no tengan ese efecto ni lo hayan sufrido recientemente
            return enemigos.Any(e =>
                !e.efectosActivos.Exists(ef => ef.tipo == tipoEfecto) &&
                (!e.turnosDesdeUltimoEfecto.ContainsKey(tipoEfecto) || e.turnosDesdeUltimoEfecto[tipoEfecto] >= 2));
        });


        if (debuffs.Count > 0 && Random.value < 0.6f)
            return debuffs[Random.Range(0, debuffs.Count)];


        // 4. Ataques ofensivos, incluyendo ataque en área
        var ofensivas = cartas.FindAll(c =>
            (c.accion.mensaje == "CambiarVida" && c.accion.argumento < 0) ||
            c.accion.mensaje == "RobarSalud" ||
            c.accion.mensaje == "AplicarEfectoGlobal");

        if (ofensivas.Count > 0)
            return ofensivas[Random.Range(0, ofensivas.Count)];

        // 5. Último recurso: carta aleatoria
        return cartas[Random.Range(0, cartas.Count)];
    }


    // Estrategia de selección de objetivo
    private Luchador ElegirObjetivo(ScriptableCartas carta, Luchador lanzador)
    {
        // Si está confundido, puede atacar aliados enemigos
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

        // Lista de enemigos válidos
        var posiblesObjetivos = new List<Luchador>(FindObjectsOfType<Luchador>());
        posiblesObjetivos = posiblesObjetivos.FindAll(l => l.sigueVivo && l.Aliado != lanzador.Aliado);

        if (posiblesObjetivos.Count == 0)
        {
            Debug.LogWarning("EnemyAI: No hay enemigos válidos para atacar.");
            return null;
        }

        if (carta.accion.objetivoEsElEquipo)
            return lanzador;

        // Reutiliza objetivo anterior si aún está vivo
        memoriaDeObjetivos.TryGetValue(lanzador, out var ultimoObjetivo);
        if (posiblesObjetivos.Count > 1 && ultimoObjetivo != null && posiblesObjetivos.Contains(ultimoObjetivo))
            posiblesObjetivos.Remove(ultimoObjetivo);

        Luchador objetivoElegido;

        // A veces prioriza enemigo con menos vida
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

    // Ejecuta la acción seleccionada con una corrutina
    private IEnumerator EjecutarAccion(Accion accion, Luchador lanzador, Luchador objetivo)
    {
        if (lanzador != null)
        {
            Debug.Log(lanzador.name);
        }

        yield return lanzador.EjecutarAccion(accion, objetivo, null, lanzador);

        // Espera breve para que se note el final de la animación antes de avanzar
        yield return new WaitForSeconds(2.5f); // ajusta el tiempo a tu gusto

        TurnoFinalizado = true;
    }


    // Limpia la memoria de objetivos para evitar repeticiones entre turnos
    public void ReiniciarMemoriaDeObjetivos()
    {
        memoriaDeObjetivos.Clear();
    }
}
