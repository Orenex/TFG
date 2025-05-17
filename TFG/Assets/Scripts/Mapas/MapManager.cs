using UnityEngine;

public class MapManager : MonoBehaviour
{
    public NodosMapas nodoInicial;

    void Start()
    {
        nodoInicial.ActivateNode();
    }
}
