using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar;
    public string escenaBtnAjustes;

    public void Iniciar()
    {
        SceneManager.LoadScene(escenaBtnIniciar);
    }
    public void Ajustes()
    {
        SceneManager.LoadScene(escenaBtnIniciar);
    }

    public void Salir()
    {
        Debug.Log("Me salgo");
        Application.Quit();

    }




}
