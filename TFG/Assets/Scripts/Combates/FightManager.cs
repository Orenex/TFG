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
    [SerializeField] private Mano manoJugador;
    [SerializeField] private Deck deck;

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
            {
                ordenTurnos.Enqueue(luchador);
            }
        }
    }

    private IEnumerator GestionarTurnos()
    {
        if (luchadorActual.Aliado)
        {
            deck.ActivarVisual(true); // Mostrar mazo
            deck.SetCardCollection(luchadorActual.cartasDisponibles);
            deck.RobarCartas(5);

            estado = EstadoCombate.EsperandoTurno;

            // Espera a que el jugador juegue su turno
            yield return new WaitUntil(() => estado == EstadoCombate.FinTurno);
        }
        else
        {
            deck.ActivarVisual(false); // Ocultar mazo
            estado = EstadoCombate.EjecutandoAccion;

            yield return new WaitForSeconds(1f); // Pequeña pausa opcional

            var aliado = BuscarAliadoAleatorio();
            if (aliado == null)
            {
                estado = EstadoCombate.FinTurno;
                yield break;
            }

            if (luchadorActual.cartasDisponibles.CartasEnLaColeccion.Count > 0)
            {
                var carta = luchadorActual.cartasDisponibles.CartasEnLaColeccion[0];

                if (carta?.accion != null)
                {
                    carta.accion.Aplicar(luchadorActual, aliado);
                    yield return new WaitForSeconds(0.5f); // Pausa opcional tras aplicar
                }
            }

            estado = EstadoCombate.FinTurno;

        }

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

             Debug.Log($"Turno de: {luchadorActual.nombre}");

             if (luchadorActual.cartasDisponibles != null)
             {
                 deck.SetCardCollection(luchadorActual.cartasDisponibles);
                 deck.RobarCartas(5);
             }

             if (luchadorActual.Aliado)
             {
                 estado = EstadoCombate.EsperandoTurno;
                 yield return new WaitUntil(() => estado == EstadoCombate.FinTurno);
             }
             else
             {
                 estado = EstadoCombate.EjecutandoAccion;

                 if (luchadorActual.Acciones.Count > 0)
                 {
                     Luchador objetivo = BuscarObjetivoAleatorio();

                     if (objetivo != null)
                     {
                         yield return luchadorActual.EjecutarAccion(luchadorActual.Acciones[0], objetivo);
                     }
                     else
                     {
                         Debug.LogWarning("No se encontró objetivo válido para el enemigo.");
                     }
                 }

                 estado = EstadoCombate.FinTurno;
             }

            ordenTurnos.Enqueue(luchadorActual);
            luchadorActual = null; // limpia para el siguiente turno
            yield return null;
        }

       

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

    private Luchador BuscarObjetivoAleatorio()
    {
        var posibles = luchadores.FindAll(l => l.Aliado && l.sigueVivo);
        return posibles.Count > 0 ? posibles[Random.Range(0, posibles.Count)] : null;
    }

    public Luchador GetLuchadorActual()
    {
        return luchadorActual;
    }
    private Luchador BuscarAliadoAleatorio()
    {
        var aliados = luchadores.FindAll(l => l.Aliado && l.sigueVivo);
        return aliados.Count > 0 ? aliados[Random.Range(0, aliados.Count)] : null;
    }

}
