using UnityEngine;

[CreateAssetMenu(fileName = "Cartas", menuName = "Scriptable Objects/Cartas")]
public class Cartas : ScriptableObject
{
    [Header("Textos")]
    public string nombreCarta;
    public int coste;
    public int tipo; //0 = ataque, 1 = habilidad, 2 = eterna 
    public string efecto;
    public string descripcion;

    [Header("Color")]
    public int colores; // 0 = rojo, 1 = azul, 2 = amarillo 3 = verde 

    [Header("Disenio")]
    public Sprite imagen;
}
