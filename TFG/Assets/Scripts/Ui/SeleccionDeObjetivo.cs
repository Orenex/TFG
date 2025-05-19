using UnityEngine;

public class SeleccionDeObjetivo : MonoBehaviour
{
    public static SeleccionDeObjetivo Instance { get; private set; }

    private Luchador objetivoActual;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SeleccionarObjetivo(Luchador nuevoObjetivo)
    {
        objetivoActual = nuevoObjetivo;
    }

    public Luchador ObtenerObjetivoActual()
    {
        return objetivoActual;
    }
}
