using UnityEngine;

public class ConseguirOro : MonoBehaviour
{
    public int oroExtra; // Cantidad de oro a agregar

    void Start()
    {
        if (InventarioJugador.Instance != null)
        {
            // Agrega el oro al inventario del jugador
            InventarioJugador.Instance.AgregarOro(oroExtra);
        }
    }
}

// Este script puede colocarse en objetos de recompensa automática como cofres, nodos de evento
