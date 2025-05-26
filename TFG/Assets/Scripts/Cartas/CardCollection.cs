using System.Collections.Generic;
using UnityEngine;

// ScriptableObject que representa una colección de cartas.
// Utilizado para definir conjuntos de cartas que se pueden asignar a un luchador o mazo.
[CreateAssetMenu(fileName = "NuevaColeccionCartas", menuName = "Cartas/Colección")]
public class CardCollection : ScriptableObject
{
    // Lista de cartas que componen esta colección.
    public List<ScriptableCartas> CartasEnLaColeccion = new();
}
