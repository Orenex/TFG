using UnityEngine;

// Enum para categorizar los diferentes tipos de cartas disponibles.
public enum TipoCarta
{
    Ataque,
    Curacion,
    Buff,
    Debuff,
    Duplicadora,
    Ultimate
}

// ScriptableObject que define los datos y comportamientos de una carta individual.
[CreateAssetMenu(fileName = "NuevaCarta", menuName = "Cartas/Carta")]
public class ScriptableCartas : ScriptableObject
{
    public string nombreCarta;              // Nombre de la carta
    public string descripcion;              // Descripción breve de lo que hace la carta
    public string efecto;                   // Texto descriptivo del efecto de la carta
    public TipoCarta tipo;                  // Tipo de la carta según el enum definido arriba

    public Accion accion;                   // Acción principal que ejecuta la carta
    public Accion accionSecundaria;        // Acción secundaria opcional

    public Sprite imagen;                   // Imagen asociada a la carta
    public int costoSanidad;               // Costo en sanidad para usar la carta
    public RecursoCoste tipoCoste = RecursoCoste.Sanidad; // Tipo de recurso usado (por ahora solo sanidad)
}
