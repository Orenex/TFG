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

    public List<EstadoLuchador> estados = new();

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject);
    }

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

    public void RestaurarTodos()
    {
        foreach (var estado in estados)
        {
            estado.vida = 100; // O usa un valor guardado de vidaMaxima
            estado.sanidad = 100;
            estado.sigueVivo = true;
        }
    }
}
