using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Button botonReintentar;
    public Button botonSalirMenu;

    void Start()
    {
        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(ReintentarCombate);

        if (botonSalirMenu != null)
            botonSalirMenu.onClick.AddListener(SalirAlMenuPrincipal);
    }

    public void ReintentarCombate()
    {
        if (GameManager.Instance != null)
        {
            var aliados = GameObject.FindObjectsOfType<Luchador>().Where(l => l.Aliado).ToList();
            EstadoAliados.Instancia.RestaurarEstado(aliados);

            string escena = GameManager.Instance.escenaCombateActual;
            if (!string.IsNullOrEmpty(escena))
            {
                SceneManager.LoadScene(escena);
            }
            else
            {
                Debug.LogWarning("No hay escena de combate registrada en GameManager.");
            }
        }
    }

    public void SalirAlMenuPrincipal()
    {
        SceneManager.LoadScene("Menu_Inicio");
    }
}
