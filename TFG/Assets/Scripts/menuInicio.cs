using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar; 
    public string escenaBtnAjustes;  

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