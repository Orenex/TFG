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
        
    }
}
