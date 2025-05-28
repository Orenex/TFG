using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    public static InventarioJugador Instance { get; private set; }

    [Header("Economía del jugador")]
    public int oro = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool TieneOro(int cantidad) => oro >= cantidad;

    public void GastarOro(int cantidad)
    {
        oro -= cantidad;
        if (oro < 0) oro = 0;
    }

    public void AgregarOro(int cantidad)
    {
        oro += cantidad;
    }

    public int ObtenerOro() => oro;
}
