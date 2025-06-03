using System.Collections.Generic;
using UnityEngine;

// Clase encargada de la selección de objetivos enemigos durante el turno del jugador
public class SeleccionDeObjetivo : MonoBehaviour
{
    public static SeleccionDeObjetivo Instance { get; private set; }

    [Header("Marcador de selección")]
    [SerializeField] private GameObject marcadorPrefab;  // Prefab visual para indicar al objetivo

    private GameObject marcadorInstanciado; // Instancia activa del marcador visual
    private Luchador objetivoActual; // Luchador actualmente seleccionado como objetivo

    public List<Luchador> objetivosDisponibles = new();
    private int indiceSeleccionado = 0; // Índice actual en la lista de enemigos

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        if (objetivosDisponibles.Count > 0)
            SeleccionarPorIndice(0);
    }

    // Cambia la selección de objetivo en la lista
    public void CambiarSeleccion(int direccion)
    {
        if (objetivosDisponibles.Count == 0) return;

        indiceSeleccionado = (indiceSeleccionado + direccion + objetivosDisponibles.Count) % objetivosDisponibles.Count;
        SeleccionarPorIndice(indiceSeleccionado);
    }


    // Selecciona un enemigo basado en su posición en la lista
    private void SeleccionarPorIndice(int index)
    {
        if (index < 0 || index >= objetivosDisponibles.Count)
            return;

        var objetivo = objetivosDisponibles[index];
        SeleccionarObjetivo(objetivo);
    }


    // Cambia el objetivo actual y coloca el marcador visual sobre él
    public void SeleccionarObjetivo(Luchador objetivo)
    {
        if (objetivo == null || !objetivo.sigueVivo) return;

        if (objetivo == objetivoActual)
            return;

        objetivoActual = objetivo;

        if (marcadorInstanciado == null)
            marcadorInstanciado = Instantiate(marcadorPrefab);

        marcadorInstanciado.transform.SetParent(objetivo.transform);
        marcadorInstanciado.transform.position = objetivo.transform.position + Vector3.up * 2f;


        Debug.Log($"[UI] Objetivo seleccionado: {objetivo.nombre}");
    }



    // Limpia la selección de objetivo actual y elimina el marcador
    public void LimpiarSeleccion()
    {
        objetivoActual = null;

        if (marcadorInstanciado != null)
        {
            Destroy(marcadorInstanciado);
            marcadorInstanciado = null;
        }
    }

    // Devuelve el objetivo actualmente seleccionado
    public Luchador ObtenerObjetivoActual()
    {
        return objetivoActual;
    }

    public void PrepararSeleccion(TipoObjetivo tipo)
    {
        objetivosDisponibles.Clear();

        var todos = new List<Luchador>(FindObjectsOfType<Luchador>());

        switch (tipo)
        {
            case TipoObjetivo.Aliado:
                objetivosDisponibles = todos.FindAll(l => l.Aliado && l.sigueVivo);
                break;
            case TipoObjetivo.Enemigo:
                objetivosDisponibles = todos.FindAll(l => !l.Aliado && l.sigueVivo);
                break;
            case TipoObjetivo.Propio:
                var actual = TurnManager.Instance.Actual;
                if (actual != null && actual.sigueVivo)
                    objetivosDisponibles.Add(actual);
                break;
        }

        indiceSeleccionado = 0;

        if (objetivosDisponibles.Count > 0)
            SeleccionarPorIndice(0);
        else
            LimpiarSeleccion(); // Nada seleccionable
    }




}
