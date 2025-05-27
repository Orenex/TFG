using UnityEngine;

// Clase encargada de activar el nodo inicial del mapa o restaurar el último visitado
public class MapManager : MonoBehaviour
{
    public NodosMapas nodoInicial; // Nodo inicial por defecto en el mapa

    void Start()
    {
        // Si hay una partida guardada con un nodo anterior...
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.lastNodeID))
        {
            var allNodes = FindObjectsOfType<NodosMapas>();
            foreach (var node in allNodes)
            {
                // Encuentra y activa el nodo anterior
                if (node.nodeID == GameManager.Instance.lastNodeID)
                {
                    node.ActivateNode();
                    break;
                }
            }
        }
        else
        {
            // Si no hay nodo anterior, activa el nodo inicial por defecto
            nodoInicial.ActivateNode();
        }
    }
}
