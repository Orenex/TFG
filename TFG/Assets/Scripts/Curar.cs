using UnityEngine;

public class Curar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EstadoAliados.Instancia.RestaurarTodos();
    }
  
}
