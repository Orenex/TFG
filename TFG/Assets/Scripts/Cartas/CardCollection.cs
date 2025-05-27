using System.Collections.Generic;
using UnityEngine;

// ScriptableObject que representa una colecci�n de cartas
// Utilizado para definir conjuntos de cartas que pueden asignarse a luchadores o mazos
[CreateAssetMenu(fileName = "NuevaColeccionCartas", menuName = "Cartas/Colecci�n")]
public class CardCollection : ScriptableObject
{
    // Lista de cartas que componen esta colecci�n
    public List<ScriptableCartas> CartasEnLaColeccion = new();
}
