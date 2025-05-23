using System.Collections.Generic;
using UnityEngine;

public class SeleccionDeObjetivo : MonoBehaviour
{
    public static SeleccionDeObjetivo Instance { get; private set; }

    [Header("Marcador de selección")]
    [SerializeField] private GameObject marcadorPrefab;

    private GameObject marcadorInstanciado;
    private Luchador objetivoActual;

    private List<Luchador> enemigos = new();
    private int indiceSeleccionado = 0;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        enemigos = new List<Luchador>(FindObjectsOfType<Luchador>());
        enemigos = enemigos.FindAll(e => !e.Aliado && e.sigueVivo);

        if (enemigos.Count > 0)
            SeleccionarPorIndice(0);
    }

    private void Update()
    {
        if (enemigos.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            CambiarSeleccion(-1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CambiarSeleccion(1);
        }
    }

    public void CambiarSeleccion(int direccion)
    {
        if (enemigos.Count == 0) return;

        indiceSeleccionado = (indiceSeleccionado - direccion + enemigos.Count) % enemigos.Count;
        SeleccionarPorIndice(indiceSeleccionado);
    }

    private void SeleccionarPorIndice(int index)
    {
        var objetivo = enemigos[index];
        SeleccionarObjetivo(objetivo);
    }

    public void SeleccionarObjetivo(Luchador objetivo)
    {
        if (objetivo == null || !objetivo.sigueVivo) return;

        objetivoActual = objetivo;

        if (marcadorInstanciado != null)
            Destroy(marcadorInstanciado);

        marcadorInstanciado = Instantiate(marcadorPrefab, objetivo.transform);
        marcadorInstanciado.transform.localPosition = Vector3.up * 2.5f;

        Debug.Log($"Objetivo seleccionado: {objetivo.nombre}");
    }

    public void LimpiarSeleccion()
    {
        objetivoActual = null;

        if (marcadorInstanciado != null)
        {
            Destroy(marcadorInstanciado);
            marcadorInstanciado = null;
        }
    }

    public Luchador ObtenerObjetivoActual()
    {
        return objetivoActual;
    }
}
