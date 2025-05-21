using UnityEngine;

public class SeleccionDeObjetivo : MonoBehaviour
{
    public static SeleccionDeObjetivo Instance { get; private set; }

    private Luchador objetivoActual;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void SeleccionarObjetivo(Luchador luchador)
    {
        if (luchador == null || !luchador.sigueVivo)
        {
            Debug.LogWarning("Objetivo inválido.");
            return;
        }

        if (objetivoActual == luchador)
        {
            Debug.Log("Reseleccionado mismo objetivo. Limpiando.");
            LimpiarSeleccion();
            return;
        }

        objetivoActual = luchador;
        Debug.Log("Nuevo objetivo seleccionado: " + luchador.nombre);
    }

    public void LimpiarSeleccion()
    {
        objetivoActual = null;
        var selector = FindObjectOfType<SeleccionadorDeEnemigo3D>();
        if (selector != null)
            selector.OcultarIndicador();
    }

    public Luchador ObtenerObjetivoActual()
    {
        if (objetivoActual == null || !objetivoActual.sigueVivo)
            return null;

        return objetivoActual;
    }
}
