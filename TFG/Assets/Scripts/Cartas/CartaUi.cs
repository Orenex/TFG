using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CartaUi : MonoBehaviour
{
    #region Fields and Properties
    private Carta carta;

    [Header("Prefab elements")]
    [SerializeField] private Image ImagenCarta;
    [SerializeField] private TextMeshProUGUI Coste;
    [SerializeField] private TextMeshProUGUI Nombre;
    [SerializeField] private TextMeshProUGUI Tipo;
    [SerializeField] private TextMeshProUGUI Efecto;
    [SerializeField] private TextMeshProUGUI Descripcion;

    [Header("Sprite Assets")]

    private readonly string EFFECTTYPE_ATAQUE = "Ataque";
    private readonly string EFFECTTYPE_HABILIDAD = "Habilidad";
    private readonly string EFFECTTYPE_ETERNA = "Eterna";

    #endregion

    #region Methods
    private void Awake()
    {
        carta = GetComponent<Carta>();
        SetCardUI();
    }

    private void OnValidate()
    {
        Awake();
    }
    private void SetCardUI()
    {
        if (carta != null && carta.InfoCarta != null)
        {
            SetCardText();
        }
    }
    private void SetCardText()
    {
        SetCardEffectTypeText();
        Nombre.text = carta.InfoCarta.nombreCarta;
        Descripcion.text = carta.InfoCarta.descripcion;
        Efecto.text = carta.InfoCarta.efecto;
        Coste.text = carta.InfoCarta.coste.ToString();
        ImagenCarta.sprite = carta.InfoCarta.imagen;
    }
    private void SetCardEffectTypeText()
    {
        switch (carta.InfoCarta.tipo)
        {
            case TipoCarta.Ataque:
                Tipo.text = EFFECTTYPE_ATAQUE;
                break;
            case TipoCarta.Habilidad:
                Tipo.text = EFFECTTYPE_HABILIDAD;
                break;
            case TipoCarta.Eterna:
                Tipo.text = EFFECTTYPE_ETERNA;
                break;
        }
    }
    #endregion
}
