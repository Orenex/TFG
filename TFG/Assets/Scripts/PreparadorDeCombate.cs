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

    public Luchador marceau;
    int vidaExtra = InventarioJugador.Instance.vidaExtraArmadura; // Vida extra del inventario del jugador

    void Start()
    {
        int vidaExtra = InventarioJugador.Instance.vidaExtraArmadura;

        // Detectar automáticamente todos los luchadores aliados en la escena
        Luchador[] aliados = FindObjectsOfType<Luchador>();
        foreach (var luchador in aliados)
        {
            if (!luchador.Aliado) continue;

            // Asignar mazo según nombre y mejora
            switch (luchador.nombre)
            {
                case "Glen":
                    luchador.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Glen") ? glenPlus : glenNormal;
                    break;

                case "Jack":
                    luchador.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Jack") ? jackPlus : jackNormal;
                    break;

                case "Pagliacci":
                    luchador.cartasDisponibles = InventarioJugador.Instance.EsMazoMejorado("Pagliacci") ? pagliacciPlus : pagliacciNormal;
                    break;
            }

            // Aplicar vida extra si hay
            luchador.vidaMaxima += vidaExtra;
            luchador.vida += vidaExtra;
        }
    }

}
