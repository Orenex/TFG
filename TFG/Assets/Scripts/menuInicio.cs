using UnityEngine;
using UnityEngine.SceneManagement;

// Controla el men� principal del juego, con botones para iniciar, ajustes o salir
public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar;  // Nombre de la escena principal del juego
    public string escenaBtnAjustes;  // (No se usa aqu�, se carga la misma escena que Iniciar)

    // M�todo llamado al presionar el bot�n de Iniciar
    public void Iniciar()
    {
        SceneManager.LoadScene(escenaBtnIniciar);
    }

    // M�todo para ir a ajustes (actualmente carga la misma escena que iniciar)
    public void Ajustes()
    {
        SceneManager.LoadScene(escenaBtnIniciar);
    }

    // Cierra la aplicaci�n
    public void Salir()
    {
        Debug.Log("Me salgo");
        Application.Quit();
    }
}

// Este script se asigna a los botones del men� inicial