using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NodosMapas : MonoBehaviour
{
    public Button zonas;
    public List<NodosMapas> nodosConectados;
    public List<NodosMapas> caminosdesactivados;  // Nodos que deben desactivarse si este es elegido
    public bool isActive = false;
    public bool isVisited = false;
    public string escena;

    void Start()
    {
        zonas.onClick.AddListener(OnNodeClicked);
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

        // Activar nodos conectados
        foreach (NodosMapas node in nodosConectados)
        {
            node.ActivateNode();
        }

        // Desactivar alternativas exclusivas
        foreach (NodosMapas alt in caminosdesactivados)
        {
            alt.DeactivateNode();
        }

        // Cargar la escena asociada
        if (!string.IsNullOrEmpty(escena))
        {
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
}
