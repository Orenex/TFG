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

    private List<Luchador> enemigos = new(); // Lista de enemigos disponibles como objetivo
    private int indiceSeleccionado = 0; // Índice actual en la lista de enemigos

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        // Inicializa la lista de enemigos válidos al comienzo del combate
        enemigos = new List<Luchador>(FindObjectsOfType<Luchador>());
        enemigos = enemigos.FindAll(e => !e.Aliado && e.sigueVivo);

        if (enemigos.Count > 0)
            SeleccionarPorIndice(0);
    }

    private void Update()
    {
        if (enemigos.Count == 0) return;

        // Cambia de objetivo con teclas A y D
        if (Input.GetKeyDown(KeyCode.A))
        {
            CambiarSeleccion(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CambiarSeleccion(1);
        }
    }

    // Cambia la selección de objetivo en la lista
    public void CambiarSeleccion(int direccion)
    {
        if (enemigos.Count == 0) return;

        indiceSeleccionado = (indiceSeleccionado - direccion + enemigos.Count) % enemigos.Count;
        SeleccionarPorIndice(indiceSeleccionado);
    }

    // Selecciona un enemigo basado en su posición en la lista
    private void SeleccionarPorIndice(int index)
    {
        var objetivo = enemigos[index];
        SeleccionarObjetivo(objetivo);
    }

    // Cambia el objetivo actual y coloca el marcador visual sobre él
    public void SeleccionarObjetivo(Luchador objetivo)
    {
        if (objetivo == null || !objetivo.sigueVivo) return;

        objetivoActual = objetivo;

        if (marcadorInstanciado != null)
            Destroy(marcadorInstanciado);

        marcadorInstanciado = Instantiate(marcadorPrefab, objetivo.transform);
        marcadorInstanciado.transform.localPosition = Vector3.up * 2.5f; // Coloca el marcador sobre el objetivo

        Debug.Log($"Objetivo seleccionado: {objetivo.nombre}");
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
}
