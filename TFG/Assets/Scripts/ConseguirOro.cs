using UnityEngine;

public class ConseguirOro : MonoBehaviour
{
    public int oroExtra;
    void Start()
    {
        if (InventarioJugador.Instance != null)
        {
            InventarioJugador.Instance.AgregarOro(oroExtra);
        }
    }

  
}
