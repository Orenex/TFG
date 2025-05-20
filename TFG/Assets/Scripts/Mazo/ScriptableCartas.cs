using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Carta", menuName = "Scriptable Objects/Carta")]
public class ScriptableCartas : ScriptableObject
{
    [Header("Informaci�n b�sica")]
    [field: SerializeField] public string nombreCarta { get; private set; }
    [field: SerializeField, TextArea] public string descripcion { get; private set; }
    [field: SerializeField] public int coste { get; private set; }
    [field: SerializeField] public Sprite imagen { get; private set; }
    [field: SerializeField] public string efecto { get; private set; }

    [Header("Clasificaci�n")]
    [field: SerializeField] public TipoCarta tipo { get; private set; }
    [field: SerializeField] public CartaColor colores { get; private set; }

    [Header("Acci�n que ejecuta")]
    [field: SerializeField] public Accion accion { get; private set; }
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
