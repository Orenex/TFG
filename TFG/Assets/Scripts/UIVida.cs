using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Maneja la interfaz visual de vida, sanidad y estados alterados del luchador
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

    public Luchador luchador; // Referencia al luchador asociado

    void Start()
    {
        // Oculta todos los íconos de efectos al iniciar
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

    // Actualiza las barras y textos de vida y sanidad
    public void ActualizarUI()
    {
        if (luchador == null) return;

        float porcentajeVida = (float)luchador.vida / luchador.vidaMaxima;
        Vida.fillAmount = porcentajeVida;
        textoVida.text = $"{luchador.vida}/{luchador.vidaMaxima}";

        float porcentajeSanidad = (float)luchador.sanidad / luchador.sanidadMaxima;
        Sanidad.fillAmount = porcentajeSanidad;
        textoSanidad.text = $"{luchador.sanidad}/{luchador.sanidadMaxima}";
    }

    // Activa los íconos de estados alterados activos del luchador
    public void ActualizarEfectosActivos()
    {
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
                case TipoEfecto.Furia:
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
            }
        }
    }
}

// Este script debe asignarse a un prefab de UI para mostrar la salud y estados activos en combate