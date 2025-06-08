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