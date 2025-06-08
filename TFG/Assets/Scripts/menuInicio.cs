using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar;

    private void Start()
    {
       
    }
    // M�todo llamado al presionar el bot�n de Iniciar
    public void Iniciar()
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