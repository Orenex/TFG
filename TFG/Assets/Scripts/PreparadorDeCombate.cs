using UnityEngine;

public class PreparadorDeCombate : MonoBehaviour
{
    public Luchador glen;
    public CardCollection glenNormal;
    public CardCollection glenPlus;

    public Luchador jack;
    public CardCollection jackNormal;
    public CardCollection jackPlus;

    public Luchador pagliacci;
    public CardCollection pagliacciNormal;
    public CardCollection pagliacciPlus;

    void Start()
    {
        glen.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Glen") ? glenPlus : glenNormal;
        jack.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Jack") ? jackPlus : jackNormal;
        pagliacci.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Pagliacci") ? pagliacciPlus : pagliacciNormal;
    }
}
