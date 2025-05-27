using UnityEngine;

// Clase est�tica que mantiene datos simples entre escenas, como la posici�n del jugador
public static class GameState
{
    public static Vector3 lastPlayerPosition = Vector3.zero; // �ltima posici�n guardada del jugador
    public static bool hasSavedPosition = false;             // Indica si hay una posici�n v�lida guardada
}
