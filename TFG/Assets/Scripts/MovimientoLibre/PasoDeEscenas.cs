using UnityEngine;
using UnityEngine.SceneManagement;


// Clase que permite al jugador cambiar de escena al entrar en una zona
public class PasoDeEscenas : MonoBehaviour
{
    public string sceneToLoad;         // Nombre de la escena a cargar
    public GameObject messageUI;       // UI que muestra el mensaje "Pulsa E para entrar"

    private bool isPlayerInside = false; // Indica si el jugador está dentro del trigger

    void Start()
    {
        // Oculta el mensaje al iniciar
        if (messageUI != null)
            messageUI.SetActive(false);
    }

    void Update()
    {
        // Si el jugador está dentro y presiona E, se cambia de escena
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Guarda la posición del jugador antes de cambiar de escena
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                GameState.lastPlayerPosition = player.transform.position;
                GameState.hasSavedPosition = true;
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }

    // Detecta cuando el jugador entra en el área de interacción
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (messageUI != null)
                messageUI.SetActive(true);
        }
    }

    // Detecta cuando el jugador sale del área de interacción
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
