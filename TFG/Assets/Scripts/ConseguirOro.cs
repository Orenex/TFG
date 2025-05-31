using UnityEngine;

// Clase que al iniciar agrega oro adicional al inventario del jugador
// Se puede usar como recompensa automática al llegar a un punto del juego
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

// Este script puede colocarse en objetos de recompensa automática
// como cofres, nodos de evento o NPCs que otorgan oro
