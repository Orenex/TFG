using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public static FightManager Instance { get; private set; }

    [Header("Participantes")]
    [SerializeField] private List<Luchador> luchadores = new();

    [Header("Turnos")]
    private Queue<Luchador> ordenTurnos = new();
    private Luchador luchadorActual;

    [Header("Control jugador")]
    [SerializeField] public Mano manoJugador;
    [SerializeField] public Deck deck;

    private enum EstadoCombate
    {
        EsperandoTurno,
        EjecutandoAccion,
        FinTurno
    }

    private EstadoCombate estado = EstadoCombate.EsperandoTurno;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        InicializarOrdenTurnos();
        if (ordenTurnos.Count > 0)
            luchadorActual = ordenTurnos.Dequeue();

        StartCoroutine(GestionarTurnos());
    }

    private void InicializarOrdenTurnos()
    {
        ordenTurnos.Clear();

        foreach (var luchador in luchadores)
        {
            if (luchador != null && luchador.sigueVivo)
                ordenTurnos.Enqueue(luchador);
        }
    }

    private IEnumerator GestionarTurnos()
    {
        while (true)
        {
            if (ordenTurnos.Count == 0)
            {
                InicializarOrdenTurnos();
                yield return null;
            }

            luchadorActual = ordenTurnos.Dequeue();

            if (luchadorActual == null || !luchadorActual.sigueVivo)
                continue;

            
            SeleccionDeObjetivo.Instance.LimpiarSeleccion();
            ReiniciarSelectorDeEnemigos();

            luchadorActual.AplicarEfectosPorTurno();

            Debug.Log($"Turno de: {luchadorActual.nombre}");

            if (luchadorActual.Aliado)
            {
                deck.ActivarVisual(true);
                deck.SetCardCollection(luchadorActual.cartasDisponibles);
                deck.RobarCartas(5);

                manoJugador.ReiniciarDescartesDelTurno();

                estado = EstadoCombate.EsperandoTurno;
                yield return new WaitUntil(() => estado == EstadoCombate.FinTurno);
            }
            else
            {
                deck.ActivarVisual(false);
                estado = EstadoCombate.EjecutandoAccion;
                yield return new WaitForSeconds(1f);

                EjecutarTurnoEnemigo(luchadorActual);
                yield return new WaitForSeconds(1f);

                estado = EstadoCombate.FinTurno;
            }

            ordenTurnos.Enqueue(luchadorActual);
            luchadorActual = null;
            yield return null;
        }
    }

    private void ReiniciarSelectorDeEnemigos()
    {
        var selector = FindObjectOfType<SeleccionadorDeEnemigo3D>();
        if (selector != null)
        {
            if (!selector.gameObject.activeInHierarchy)
            {
                Debug.Log("Reactivando GameObject del selector.");
                selector.gameObject.SetActive(true);
            }

            if (!selector.enabled)
            {
                Debug.Log("Reactivando script SeleccionadorDeEnemigo3D.");
                selector.enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("No se encontró un objeto con SeleccionadorDeEnemigo3D.");
        }
    }

    private void EjecutarTurnoEnemigo(Luchador enemigo)
    {
        var cartasDisponibles = enemigo.cartasDisponibles.CartasEnLaColeccion;

        var jugables = cartasDisponibles.FindAll(c =>
            c != null &&
            ((c.accion.tipoCoste == RecursoCoste.Mana && enemigo.mana >= c.accion.costoMana) ||
             (c.accion.tipoCoste == RecursoCoste.Sanidad && enemigo.sanidad >= c.accion.costoMana))
        );

        if (jugables.Count == 0) return;

        ScriptableCartas cartaElegida = ElegirCartaInteligente(jugables, enemigo);
        Luchador objetivo = ElegirObjetivoParaCarta(cartaElegida, enemigo);

        if (objetivo != null)
        {
            StartCoroutine(enemigo.EjecutarAccion(cartaElegida.accion, objetivo));
        }
    }

    private ScriptableCartas ElegirCartaInteligente(List<ScriptableCartas> cartas, Luchador lanzador)
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

    private Luchador ElegirObjetivoParaCarta(ScriptableCartas carta, Luchador lanzador)
    {
        if (carta.accion.objetivoEsElEquipo)
            return lanzador;

        if (carta.accion.mensaje == "CambiarVida" && carta.accion.argumento < 0)
        {
            var enemigos = luchadores.FindAll(l => l.Aliado && l.sigueVivo);
            return enemigos.Count > 0 ? enemigos[Random.Range(0, enemigos.Count)] : null;
        }

        if (carta.accion.mensaje == "CambiarVida" && carta.accion.argumento > 0)
        {
            var aliadosHeridos = luchadores.FindAll(l => !l.Aliado && l.sigueVivo && l.vida < 15);
            return aliadosHeridos.Count > 0 ? aliadosHeridos[Random.Range(0, aliadosHeridos.Count)] : lanzador;
        }

        return lanzador;
    }

    public void EjecutarAccionJugador(Accion accion, Luchador objetivo)
    {
        if (estado != EstadoCombate.EsperandoTurno)
            return;

        estado = EstadoCombate.EjecutandoAccion;
        StartCoroutine(EjecutarAccion(accion, objetivo));
    }

    private IEnumerator EjecutarAccion(Accion accion, Luchador objetivo)
    {
        yield return luchadorActual.EjecutarAccion(accion, objetivo);
        estado = EstadoCombate.FinTurno;
    }

    public Luchador GetLuchadorActual()
    {
        return luchadorActual;
    }

    public void TerminarTurno()
    {
        estado = EstadoCombate.FinTurno;
    }
}
