using System.Collections.Generic;
using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    public static InventarioJugador Instance;

    public int oroActual = 100;
    public List<string> mazosMejorados = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CargarMazosMejorados();
        }
    }
    //Esto hay que eliminarlo cuando se entregue
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            mazosMejorados.Clear();
            oroActual = 100; // o el valor inicial que uses
            Debug.Log("Inventario reseteado");
        }
    }

    public bool TieneOro(int cantidad) => oroActual >= cantidad;

    public void GastarOro(int cantidad)
    {
        oroActual -= cantidad;
        Debug.Log("Oro restante: " + oroActual);
    }

    public int ObtenerOro() => oroActual;

    public void MarcarMazoComoMejorado(string id)
    {
        Debug.Log("Intentando marcar como comprado: '" + id + "'");

        if (!mazosMejorados.Contains(id))
        {
            mazosMejorados.Add(id);
            PlayerPrefs.SetInt("MazoMejorado_" + id, 1);
            Debug.Log("Mazo marcado como mejorado y guardado: " + id);
        }
        else
        {
            Debug.LogWarning("El mazo ya estaba marcado como comprado: " + id);
        }
    }

    public bool EsMazoMejorado(string id)
    {
        bool resultado = mazosMejorados.Contains(id);
        Debug.Log($"¿'{id}' está en mazos mejorados?  {resultado}");
        return resultado;
    }

    public void CargarMazosMejorados()
    {
        string[] ids = new[] { "Glen", "Jack", "Pagliacci" };

        foreach (var id in ids)
        {
            if (PlayerPrefs.GetInt("MazoMejorado_" + id, 0) == 1)
            {
                mazosMejorados.Add(id);
                Debug.Log("Cargado desde PlayerPrefs: " + id);
            }
        }
    }

    public void DebugMazosComprados()
    {
        Debug.Log("Mazos mejorados actuales:");
        foreach (var id in mazosMejorados)
        {
            Debug.Log("- " + id);
        }
    }

    public void AgregarOro(int cantidad)
    {
        oroActual += cantidad;
        Debug.Log($"Oro actual tras bonus: {oroActual}");
    }

}
