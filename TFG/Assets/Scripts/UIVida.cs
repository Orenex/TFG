using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIVida : MonoBehaviour
{
    public Image Paralisis;
    public Image Furia;
    public Image Asqueado;
    public Image Sangrado;
    public Image CompartirDanio;
    public Image Vida;
    public Image Sanidad;
    public TextMeshProUGUI textoVida;
    public TextMeshProUGUI textoSanidad;


    public Luchador luchador;
    void Start()
    {
        Paralisis.gameObject.SetActive(false);
        Furia.gameObject.SetActive(false);
        Asqueado.gameObject.SetActive(false);
        Sangrado.gameObject.SetActive(false);
        CompartirDanio.gameObject.SetActive(false);

        
    }


    void Update()
    {
        ActualizarUI();
        ActualizarEfectosActivos();
    }


    public void ActualizarUI()
    {
        if (luchador == null) return;

        // Actualiza la barra y texto de vida
        float porcentajeVida = (float)luchador.vida / luchador.vidaMaxima;
        Vida.fillAmount = porcentajeVida;
        textoVida.text = $"{luchador.vida}/{luchador.vidaMaxima}";

        // Actualiza la barra y texto de sanidad
        float porcentajeSanidad = (float)luchador.sanidad / luchador.sanidadMaxima;
        Sanidad.fillAmount = porcentajeSanidad;
        textoSanidad.text = $"{luchador.sanidad}/{luchador.sanidadMaxima}";
    }

    public void ActualizarEfectosActivos()
    {
        // Primero, desactiva todos
        Paralisis.gameObject.SetActive(false);
        Furia.gameObject.SetActive(false);
        Asqueado.gameObject.SetActive(false);
        Sangrado.gameObject.SetActive(false);
        CompartirDanio.gameObject.SetActive(false);

        foreach (var efecto in luchador.efectosActivos)
        {
            switch (efecto.tipo)
            {
                case TipoEfecto.Paralizado:
                    Paralisis.gameObject.SetActive(true);
                    break;
                case TipoEfecto.FuriaSanidad:
                    Furia.gameObject.SetActive(true);
                    break;
                case TipoEfecto.Asqueado:
                    Asqueado.gameObject.SetActive(true);
                    break;
                case TipoEfecto.Sangrado:
                    Sangrado.gameObject.SetActive(true);
                    break;
                case TipoEfecto.CompartirDaño:
                    CompartirDanio.gameObject.SetActive(true);
                    break;
                    // Agrega otros efectos si es necesario
            }
        }
    }

}
