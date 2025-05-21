using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartaUi : MonoBehaviour
{
    #region References

    [Header("Elementos UI")]
    [SerializeField] private Image imagenCarta;
    [SerializeField] private TextMeshProUGUI textoCoste;
    [SerializeField] private TextMeshProUGUI textoNombre;
    [SerializeField] private TextMeshProUGUI textoTipo;
    [SerializeField] private TextMeshProUGUI textoEfecto;
    [SerializeField] private TextMeshProUGUI textoDescripcion;
    [SerializeField] private TextMeshProUGUI textoRecurso; // NUEVO

    #endregion

    #region Public Methods

    public void ActualizarUI(ScriptableCartas data)
    {
        if (data == null)
        {
            Debug.LogError("¡ScriptableCartas en CartaUI es null!");
            return;
        }

        if (textoNombre == null || textoCoste == null || textoDescripcion == null)
        {
            Debug.LogError("Faltan referencias en CartaUi");
            return;
        }

        textoNombre.text = data.nombreCarta;
        textoDescripcion.text = data.descripcion;
        textoEfecto.text = data.efecto;
        textoCoste.text = data.coste.ToString();
        textoTipo.text = ObtenerTextoTipo(data.tipo);
        imagenCarta.sprite = data.imagen;

        if (textoRecurso != null)
        {
            textoRecurso.text = ObtenerTextoRecurso(data.tipoCoste);
        }
    }

    #endregion

    #region Helpers

    private string ObtenerTextoTipo(TipoCarta tipo)
    {
        return tipo switch
        {
            TipoCarta.Ataque => "Ataque",
            TipoCarta.Habilidad => "Habilidad",
            TipoCarta.Duplicadora => "Duplicadora",
            _ => "Desconocido"
        };
    }

    private string ObtenerTextoRecurso(RecursoCoste recurso)
    {
        return recurso switch
        {
            RecursoCoste.Mana => "Usa Maná",
            RecursoCoste.Sanidad => "Usa Sanidad",
            _ => "Sin recurso"
        };
    }

    #endregion
}
