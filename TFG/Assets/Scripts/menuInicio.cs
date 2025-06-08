using UnityEngine;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour
{
    public string escenaBtnIniciar;

    private void Start()
    {
        InventarioJugador.Instance?.ReiniciarInventario();
        ConfirmarFinCatacumbas.mazmorra1Completada = false;

        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
            GameManager.Instance = null;
        }
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