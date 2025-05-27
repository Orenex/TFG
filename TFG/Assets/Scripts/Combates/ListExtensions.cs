using System.Collections.Generic;
using UnityEngine;

// Clase de extensión que añade funcionalidades adicionales a listas genéricas
public static class ListExtensions
{
    // Extensión para mezclar los elementos de una lista de forma aleatoria (Fisher-Yates shuffle)
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count); // Elige una posición aleatoria desde i hasta el final
            (list[i], list[j]) = (list[j], list[i]); // Intercambia los elementos
        }
    }
}
