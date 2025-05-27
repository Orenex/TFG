using UnityEngine;

// Clase estática que mantiene datos simples entre escenas, como la posición del jugador
public static class GameState
{
    public static Vector3 lastPlayerPosition = Vector3.zero; // Última posición guardada del jugador
    public static bool hasSavedPosition = false;             // Indica si hay una posición válida guardada
}
