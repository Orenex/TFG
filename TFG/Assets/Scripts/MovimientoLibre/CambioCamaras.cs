using UnityEngine;
using Unity.Cinemachine;

// Cambia la prioridad de una cámara al entrar/salir de una zona, usando Cinemachine
public class CambioCamaras : MonoBehaviour
{
    public CinemachineCamera cameraToActivate; // Cámara a activar al entrar en el trigger

    // Al entrar el jugador, se aumenta la prioridad de esta cámara
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entró el jugador en zona de cámara: " + cameraToActivate.name);
            cameraToActivate.Priority = 20; // Alta prioridad para activarse
        }
    }

    // Al salir el jugador, se baja la prioridad para desactivarla
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Salió el jugador en zona de cámara: " + cameraToActivate.name);
            cameraToActivate.Priority = 0; // Baja prioridad para desactivarse
        }
    }
}
