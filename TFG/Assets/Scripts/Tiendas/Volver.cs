using UnityEngine;
using UnityEngine.SceneManagement;

// Clase que permite volver a la escena del pueblo y restaurar la posici�n del jugador
public class Volver : MonoBehaviour
{
    // Llamado por un bot�n o evento para regresar a "PuebloPrincipal"
    public void VolverAlPueblo()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Se suscribe al evento de escena cargada
        SceneManager.LoadScene("PuebloPrincipal"); // Carga la escena del pueblo
    }

    // Cuando se carga la escena, se restaura la posici�n previa si fue guardada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PuebloPrincipal" && GameState.hasSavedPosition)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                CharacterController controller = player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false; // Se desactiva el controlador para cambiar posici�n
                    player.transform.position = GameState.lastPlayerPosition; // Se mueve al punto guardado
                    controller.enabled = true; // Se reactiva el controlador
                }
            }

            GameState.hasSavedPosition = false; // Se limpia el estado de guardado
        }

        SceneManager.sceneLoaded -= OnSceneLoaded; // Se desuscribe del evento
    }
}
