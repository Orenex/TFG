using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string lastScene = "";
    public string lastNodeID = "";
    public string escenaCombateActual = ""; // NUEVO
    public List<string> enemigosActuales = new(); // NUEVO
    public Dictionary<string, NodeState> nodeStates = new();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    public class NodeState
    {
        public bool isActive;
        public bool isVisited;
    }
}
