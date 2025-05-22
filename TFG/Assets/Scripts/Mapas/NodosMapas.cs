using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NodosMapas : MonoBehaviour
{
    public string nodeID; // Asegúrate de asignar un ID único desde el Inspector
    public Button zonas;
    public List<NodosMapas> nodosConectados;
    public List<NodosMapas> caminosdesactivados;
    public bool isActive = false;
    public bool isVisited = false;
    public string escena;

    void Start()
    {
        zonas.onClick.AddListener(OnNodeClicked);

        if (GameManager.Instance != null && GameManager.Instance.nodeStates.TryGetValue(nodeID, out var state))
        {
            isActive = state.isActive;
            isVisited = state.isVisited;
        }

        UpdateButtonState();
    }

    void UpdateButtonState()
    {
        zonas.interactable = isActive && !isVisited;
    }

    void OnNodeClicked()
    {
        if (!isActive || isVisited) return;

        isVisited = true;
        zonas.interactable = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.lastNodeID = nodeID;
            SaveState();
        }

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

        if (!string.IsNullOrEmpty(escena))
        {
            if (GameManager.Instance != null)
                GameManager.Instance.lastScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(escena);
        }
    }

    public void ActivateNode()
    {
        isActive = true;
        UpdateButtonState();
    }

    public void DeactivateNode()
    {
        isActive = false;
        UpdateButtonState();
    }

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
