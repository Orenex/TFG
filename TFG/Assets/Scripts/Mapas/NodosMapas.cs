using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Clase que representa cada nodo en el mapa del juego (zona de combate o evento)
public class NodosMapas : MonoBehaviour
{
    public string nodeID; // ID único asignado desde el Inspector
    public Button zonas; // Botón que representa este nodo en la UI
    public List<NodosMapas> nodosConectados; // Nodos que se activan al visitar este
    public List<NodosMapas> caminosdesactivados; // Nodos que se desactivan al visitarlo
    public bool isActive = false;
    public bool isVisited = false;
    public string escena; // Escena que se carga al hacer clic

    public bool esUltimoNodo = false; // Si este nodo marca el final del recorrido

    void Start()
    {
        // Asigna el evento de clic al botón
        zonas.onClick.AddListener(OnNodeClicked);

        // Restaura estado del nodo desde el GameManager
        if (GameManager.Instance != null && GameManager.Instance.nodeStates.TryGetValue(nodeID, out var state))
        {
            isActive = state.isActive;
            isVisited = state.isVisited;
        }

        UpdateButtonState();
    }

    // Actualiza la interactividad del botón
    void UpdateButtonState()
    {
        zonas.interactable = isActive && !isVisited;
    }

    // Método llamado cuando se hace clic sobre el nodo
    void OnNodeClicked()
    {
        if (!isActive || isVisited) return;

        isVisited = true;
        zonas.interactable = false;

        // Guarda el nodo actual como último visitado
        if (GameManager.Instance != null)
        {
            GameManager.Instance.lastNodeID = nodeID;
            SaveState();
        }

        // Activa y desactiva nodos conectados
        foreach (NodosMapas node in nodosConectados)
        {
            node.ActivateNode();
            node.SaveState();
        }

        foreach (NodosMapas alt in caminosdesactivados)
        {
            alt.DeactivateNode();
            alt.SaveState();
        }

        // Carga la escena asociada si existe
        if (!string.IsNullOrEmpty(escena))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.lastScene = SceneManager.GetActiveScene().name;

                //Limpiar enemigos actuales para forzar randomización
                GameManager.Instance.enemigosActuales.Clear();

                // Elimina GameManager si se llega al final del camino
                if (esUltimoNodo)
                {
                    Destroy(GameManager.Instance.gameObject);
                }
            }
            GameManager.Instance.escenaCombateActual = escena;
            SceneManager.LoadScene(escena);
        }
    }

    // Activa el nodo actual
    public void ActivateNode()
    {
        isActive = true;
        UpdateButtonState();
    }

    // Desactiva el nodo actual
    public void DeactivateNode()
    {
        isActive = false;
        UpdateButtonState();
    }

    // Guarda el estado del nodo en GameManager
    public void SaveState()
    {
        if (GameManager.Instance == null) return;

        if (!GameManager.Instance.nodeStates.ContainsKey(nodeID))
            GameManager.Instance.nodeStates[nodeID] = new GameManager.NodeState();

        var state = GameManager.Instance.nodeStates[nodeID];
        state.isActive = isActive;
        state.isVisited = isVisited;
    }
}
