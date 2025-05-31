using UnityEngine;

// Clase que asigna automáticamente los mazos correctos a cada personaje al inicio del combate
// Usa el estado guardado en InventarioJugador para determinar si el mazo es mejorado o no
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
        // Asigna el mazo adecuado a cada luchador según si fue mejorado
        glen.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Glen") ? glenPlus : glenNormal;
        jack.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Jack") ? jackPlus : jackNormal;
        pagliacci.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Pagliacci") ? pagliacciPlus : pagliacciNormal;
    }
}

// Este script se ejecuta al inicio de la escena de combate y configura las cartas de cada personaje
