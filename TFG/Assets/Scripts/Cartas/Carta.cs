using UnityEngine;

[RequireComponent(typeof(CartaUi))] //pone automaticamente el script CartaUi a cualquier objeto que sea una carta
[RequireComponent(typeof(MovimientoCarta))]
public class Carta : MonoBehaviour
{
    #region Fields and Properties
    [field: SerializeField] public ScriptableCartas DataCarta { get;private set; }

    #endregion

    #region Metodos
    //establecer data de la carta al ejecutar
    public void SetUp(ScriptableCartas data)
    {
        DataCarta = data;
        GetComponent<CartaUi>().SetCardUI();
    }
    
    #endregion
}
