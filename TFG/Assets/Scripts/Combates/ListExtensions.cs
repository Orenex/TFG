using System.Collections.Generic;
using UnityEngine;

// Clase de extensi�n que a�ade funcionalidades adicionales a listas gen�ricas
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
