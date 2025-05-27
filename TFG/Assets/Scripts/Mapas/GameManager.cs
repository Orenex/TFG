using System.Collections.Generic;
using UnityEngine;

// Clase principal que mantiene el estado general del juego entre escenas
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Instancia singleton para acceso global

    public string lastScene = "";         // Nombre de la última escena activa
    public string lastNodeID = "";        // ID del último nodo del mapa visitado
    public Dictionary<string, NodeState> nodeStates = new(); // Estado de todos los nodos visitados/activados

    void Awake()
    {
        // Asegura que solo exista una instancia y persista entre escenas
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
    }

    // Clase interna que representa el estado de un nodo en el mapa
    [System.Serializable]
    public class NodeState
    {
        public bool isActive;   // Si el nodo está activo y puede ser visitado
        public bool isVisited;  // Si ya ha sido visitado anteriormente
    }
}
