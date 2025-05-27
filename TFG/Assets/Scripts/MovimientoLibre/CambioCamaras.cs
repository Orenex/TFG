using UnityEngine;
using Unity.Cinemachine;

public class CambioCamaras : MonoBehaviour
{
    public CinemachineCamera cameraToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entr� el jugador en zona de c�mara: " + cameraToActivate.name);

            cameraToActivate.Priority = 20; // M�s alta que el resto
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Salio el jugador en zona de c�mara: " + cameraToActivate.name);

            cameraToActivate.Priority = 0; // Baja la prioridad para desactivarla
        }
    }
}
