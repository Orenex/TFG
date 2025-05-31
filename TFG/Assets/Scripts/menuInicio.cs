using UnityEngine;
using UnityEngine.SceneManagement;

// Controla el menú principal del juego, con botones para iniciar, ajustes o salir
public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar;  // Nombre de la escena principal del juego
    public string escenaBtnAjustes;  // (No se usa aquí, se carga la misma escena que Iniciar)

    // Método llamado al presionar el botón de Iniciar
    public void Iniciar()
    {
        SceneManager.LoadScene(escenaBtnIniciar);
    }

    // Método para ir a ajustes (actualmente carga la misma escena que iniciar)
    public void Ajustes()
    {
        SceneManager.LoadScene(escenaBtnIniciar);
    }

    // Cierra la aplicación
    public void Salir()
    {
        Debug.Log("Me salgo");
        Application.Quit();
    }
}

// Este script se asigna a los botones del menú inicial