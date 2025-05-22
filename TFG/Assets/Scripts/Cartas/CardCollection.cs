using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NuevaColeccionCartas", menuName = "Cartas/Colección")]
public class CardCollection : ScriptableObject
{
    public List<ScriptableCartas> CartasEnLaColeccion = new();
}
