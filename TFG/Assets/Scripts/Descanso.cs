using UnityEngine;

public class Descanso : MonoBehaviour
{
    private bool isPlayerInside = false;
    public GameObject messageUI;

    void Start()
    {
        // Oculta el mensaje al iniciar
        if (messageUI != null)
            messageUI.SetActive(false);
    }

    private void Update()
    {
        // Si el jugador est� dentro y presiona 'E', se restauran todos los aliados
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            EstadoAliados.Instancia.RestaurarTodos();
            Debug.Log("Todos los aliados han sido restaurados en la hoguera.");
        }
    }

    // Detecta si el jugador entra en el �rea de descanso
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            isPlayerInside = true;
            if (messageUI != null)
                messageUI.SetActive(true);
        }
    }

    // Detecta si el jugador sale del �rea de descanso
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (messageUI != null)
                messageUI.SetActive(false);
        }
    }
}
