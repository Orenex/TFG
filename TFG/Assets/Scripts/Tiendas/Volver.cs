using UnityEngine;
using UnityEngine.SceneManagement;

public class Volver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolverAlPueblo()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("PuebloPrincipal");
    }

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
                    controller.enabled = false; // Necesario para cambiar posición con CharacterController
                    player.transform.position = GameState.lastPlayerPosition;
                    controller.enabled = true;
                }
            }

            GameState.hasSavedPosition = false;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
