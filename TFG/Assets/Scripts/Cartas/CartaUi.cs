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
    public void SetCardUI()
    {
        if (carta != null && carta.DataCarta != null)
        {
            SetCardText();
        }
    }
    private void SetCardText()
    {
        SetCardEffectTypeText();
        Nombre.text = carta.DataCarta.nombreCarta;
        Descripcion.text = carta.DataCarta.descripcion;
        Efecto.text = carta.DataCarta.efecto;
        Coste.text = carta.DataCarta.coste.ToString();
        ImagenCarta.sprite = carta.DataCarta.imagen;
    }
    private void SetCardEffectTypeText()
    {
        switch (carta.DataCarta.tipo)
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
