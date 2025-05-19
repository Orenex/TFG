using System.Collections.Generic;
using UnityEngine;

public class SeleccionadorDeEnemigo3D : MonoBehaviour
{
    [SerializeField] private GameObject indicadorPrefab;
    private GameObject indicadorInstanciado;

    private List<Luchador> enemigos = new();
    private int indiceSeleccionado = 0;

    public Luchador EnemigoSeleccionado => enemigos.Count > 0 ? enemigos[indiceSeleccionado] : null;

    private void Start()
    {
        enemigos = new List<Luchador>(FindObjectsOfType<Luchador>());
        enemigos.RemoveAll(e => e.Aliado); // Eliminar aliados

        if (enemigos.Count == 0)
        {
            Debug.LogWarning("No se encontraron enemigos para seleccionar.");
            return;
        }

        indicadorInstanciado = Instantiate(indicadorPrefab);
        indicadorInstanciado.transform.SetParent(null);
        indicadorInstanciado.SetActive(true);

        ActualizarIndicador();
        SeleccionDeObjetivo.Instance?.SeleccionarObjetivo(EnemigoSeleccionado); // Inicializar selección
    }

    private void Update()
    {
        if (enemigos.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            indiceSeleccionado = (indiceSeleccionado + 1) % enemigos.Count;
            ActualizarIndicador();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            indiceSeleccionado = (indiceSeleccionado - 1 + enemigos.Count) % enemigos.Count;
            ActualizarIndicador();
        }
    }

    private void ActualizarIndicador()
    {
        if (indicadorInstanciado == null) return;

        var enemigo = enemigos[indiceSeleccionado];
        var posicion = enemigo.transform.position + Vector3.up * 2f;
        indicadorInstanciado.transform.position = posicion;

        SeleccionDeObjetivo.Instance?.SeleccionarObjetivo(enemigo); // Actualiza el objetivo real
        Debug.Log("Enemigo seleccionado: " + enemigo.name);
    }
}
