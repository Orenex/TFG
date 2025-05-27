using UnityEngine;

// Enum que define los diferentes tipos de cartas disponibles
public enum TipoCarta
{
    Ataque,
    Curacion,
    Buff,
    Debuff,
    Duplicadora,
    Ultimate
}

// ScriptableObject que define los datos y comportamientos de una carta individual
[CreateAssetMenu(fileName = "NuevaCarta", menuName = "Cartas/Carta")]
public class ScriptableCartas : ScriptableObject
{
    public string nombreCarta;              // Nombre visible de la carta
    public string descripcion;              // Descripción general del uso
    public string efecto;                   // Texto del efecto (para UI)
    public TipoCarta tipo;                  // Tipo de la carta según el enum

    public Accion accion;                   // Acción principal que ejecuta la carta
    public Accion accionSecundaria;        // Acción secundaria (opcional)

    public Sprite imagen;                   // Imagen visual que se muestra en la UI
    public int costoSanidad;               // Costo en puntos de sanidad para poder usarla
    public RecursoCoste tipoCoste = RecursoCoste.Sanidad; // Tipo de recurso necesario (por defecto: Sanidad)
}
