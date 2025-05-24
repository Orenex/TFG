using UnityEngine;

public enum TipoCarta
{
    Ataque,
    Curacion,
    Buff,
    Debuff,
    Duplicadora,
    Ultimate
}

[CreateAssetMenu(fileName = "NuevaCarta", menuName = "Cartas/Carta")]
public class ScriptableCartas : ScriptableObject
{
    public string nombreCarta;
    public string descripcion;
    public string efecto;
    public TipoCarta tipo;

    public Accion accion;
    public Accion accionSecundaria;

    public Sprite imagen;
    public int costoSanidad;
    public RecursoCoste tipoCoste = RecursoCoste.Sanidad;
}


