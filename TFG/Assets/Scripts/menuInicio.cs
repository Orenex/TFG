using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar; 
    public string escenaBtnAjustes;  

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