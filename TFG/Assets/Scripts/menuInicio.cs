using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar;

    private void Start()
    {
       
    }
    // Método llamado al presionar el botón de Iniciar
    public void Iniciar()
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