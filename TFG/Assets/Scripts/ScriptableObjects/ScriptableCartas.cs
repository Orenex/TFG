using UnityEngine;

[CreateAssetMenu(fileName = "Cartas", menuName = "Scriptable Objects/Cartas")]
public class ScriptableCartas : ScriptableObject
{
    [field: SerializeField] public string nombreCarta { get; private set; }
    [field: SerializeField, TextArea] public string descripcion { get; private set; }
    [field: SerializeField] public int coste { get; private set; }
    [field: SerializeField] public Sprite imagen { get; private set; }
    [field: SerializeField] public string efecto { get; private set; }
    [field: SerializeField] public TipoCarta tipo { get; private set; }
    [field: SerializeField] public CartaColor colores { get; private set; }
}

public enum TipoCarta
{
    Ataque,
    Habilidad,
    Eterna,
}
public enum CartaColor
{
    Rojo,
    Azul,
    Amarillo,
    Verde,
}

