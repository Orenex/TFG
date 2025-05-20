using UnityEngine;

[RequireComponent(typeof(CartaUi))]
[RequireComponent(typeof(MovimientoCarta))]
public class Carta : MonoBehaviour
{
    #region Properties

    public ScriptableCartas DataCarta { get; private set; }


    #endregion

    #region Methods

    public void SetUp(ScriptableCartas data)
    {
        if (data == null)
        {
            Debug.LogError("¡SetUp() recibió un ScriptableCartas null!");
            return;
        }

        DataCarta = data;

        var ui = GetComponent<CartaUi>();
        if (ui == null)
        {
            Debug.LogError("CartaUi no encontrado en el prefab.");
            return;
        }

        ui.ActualizarUI(data);
    }


    #endregion
}
