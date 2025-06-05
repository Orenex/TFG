using System.Collections.Generic;
using UnityEngine;

public class EstadoAliados : MonoBehaviour
{
    public static EstadoAliados Instancia;

    [System.Serializable]
    public class EstadoLuchador
    {
        public string nombre;
        public int vida;
        public int sanidad;
        public bool sigueVivo;
    }

    public List<EstadoLuchador> estados = new(); // Lista que guarda los estados de los luchadores

    private void Awake()
    {
        // Asegura que solo haya una instancia del objeto y que no se destruya al cambiar de escena
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

    // Guarda el estado actual de los aliados en la lista
    public void GuardarEstado(List<Luchador> aliados)
    {
        estados.Clear();
        foreach (var luchador in aliados)
        {
            estados.Add(new EstadoLuchador
            {
                nombre = luchador.nombre,
                vida = luchador.vida,
                sanidad = luchador.sanidad,
                sigueVivo = luchador.sigueVivo
            });
        }
    }

    // Restaura el estado guardado a cada luchador
    public void RestaurarEstado(List<Luchador> aliados)
    {
        foreach (var luchador in aliados)
        {
            var guardado = estados.Find(e => e.nombre == luchador.nombre);
            if (guardado != null)
            {
                luchador.vida = guardado.vida;
                luchador.sanidad = guardado.sanidad;
                luchador.sigueVivo = guardado.sigueVivo;
                luchador.gameObject.SetActive(guardado.sigueVivo);
            }
        }
    }

    // Restaura completamente a todos los aliados (vida y sanidad al 100)
    public void RestaurarTodos()
    {
        foreach (var estado in estados)
        {
            estado.vida = 100;
            estado.sanidad = 100;
            estado.sigueVivo = true;
        }
    }
}
