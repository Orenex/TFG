using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PasoDeEscenas : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject messageUI; // UI que muestra "Pulsa E para entrar"

    private bool isPlayerInside = false;

    void Start()
    {
        if (messageUI != null)
            messageUI.SetActive(false); // Ocultar el mensaje al principio
    }
    void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Guardamos la posición actual del jugador antes de cambiar de escena
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                GameState.lastPlayerPosition = player.transform.position;
                GameState.hasSavedPosition = true;
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (messageUI != null)
                messageUI.SetActive(true);
        }
    }

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
