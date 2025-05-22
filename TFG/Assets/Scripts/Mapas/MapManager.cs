using UnityEngine;

public class MapManager : MonoBehaviour
{
    public NodosMapas nodoInicial;

    void Start()
    {
        if (GameManager.Instance != null && !string.IsNullOrEmpty(GameManager.Instance.lastNodeID))
        {
            var allNodes = FindObjectsOfType<NodosMapas>();
            foreach (var node in allNodes)
            {
                if (node.nodeID == GameManager.Instance.lastNodeID)
                {
                    node.ActivateNode();
                    break;
                }
            }
        }
        else
        {
            nodoInicial.ActivateNode();
        }
    }
}
