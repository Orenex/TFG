using UnityEngine;

[RequireComponent(typeof(CartaUi))]
[RequireComponent(typeof(MovimientoCarta))]
public class Carta : MonoBehaviour
{
    #region Properties

    public ScriptableCartas DataCarta { get; private set; }
    public int indiceAncla { get; internal set; }

    #endregion

    #region Methods

    /// <summary>
    /// Asigna los datos del ScriptableCartas a esta carta y actualiza su interfaz.
    /// </summary>
    public void SetUp(ScriptableCartas data)
    {
        if (data == null)
        {
            Debug.LogError("Carta.SetUp: Recibió un ScriptableCartas null.");
            return;
        }

        DataCarta = data;

        var ui = GetComponent<CartaUi>();
        if (ui != null)
        {
            ui.ActualizarUI(data);
        }
        else
        {
            Debug.LogError("Carta.SetUp: No se encontró el componente CartaUi.");
        }
    }

    #endregion
}
