using System.Collections.Generic;
using UnityEngine;

// Esta clase gestiona el inventario del jugador
public class InventarioJugador : MonoBehaviour
{
    public static InventarioJugador Instance;

    public int oroActual = 100;
    public List<string> mazosMejorados = new();
    public int vidaExtraArmadura = 0;
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
            vidaExtraArmadura = PlayerPrefs.GetInt("VidaExtraArmadura", 0);
        }
        

    }
    //Esto hay que eliminarlo cuando se entregue
    void Update()
    {
   
    }

    // Verifica si el jugador tiene oro suficiente
    public bool TieneOro(int cantidad) => oroActual >= cantidad;

    // Resta oro del inventario
    public void GastarOro(int cantidad)
    {
        oroActual -= cantidad;
        Debug.Log("Oro restante: " + oroActual);
    }
    // Devuelve el oro actual
    public int ObtenerOro() => oroActual;

    // Marca un mazo como mejorado (y lo guarda en PlayerPrefs)
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

    // Verifica si un mazo ya fue mejorado
    public bool EsMazoMejorado(string id)
    {
        bool resultado = mazosMejorados.Contains(id);
        Debug.Log($"¿'{id}' está en mazos mejorados?  {resultado}");
        return resultado;
    }

    // Carga los mazos mejorados desde PlayerPrefs
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

    // Muestra en consola los mazos mejorados actuales
    public void DebugMazosComprados()
    {
        Debug.Log("Mazos mejorados actuales:");
        foreach (var id in mazosMejorados)
        {
            Debug.Log("- " + id);
        }
    }

    // Agrega oro al jugador (puede usarse como recompensa)
    public void AgregarOro(int cantidad)
    {
        oroActual += cantidad;
        Debug.Log($"Oro actual tras bonus: {oroActual}");
    }

    // Verifica si una armadura fue comprada
    public bool EsArmaduraComprada(string id)
    {
        return PlayerPrefs.GetInt("ArmaduraComprada_" + id, 0) == 1;
    }

    // Guarda la armadura como comprada y la equipa si otorga más vida que la actual
    public void GuardarArmaduraComprada(string id, int vidaExtra)
    {
        if (!EsArmaduraComprada(id))
        {
            PlayerPrefs.SetInt("ArmaduraComprada_" + id, 1);
        }

        int vidaActual = PlayerPrefs.GetInt("VidaExtraArmadura", 0);
        if (vidaExtra > vidaActual)
        {
            PlayerPrefs.SetString("ArmaduraEquipadaID", id);
            PlayerPrefs.SetInt("VidaExtraArmadura", vidaExtra);
            vidaExtraArmadura = vidaExtra;
            Debug.Log("Armadura equipada: " + id + " con " + vidaExtra + " de vida extra.");
        }
    }
    public void ReiniciarInventario()
    {
        PlayerPrefs.DeleteAll();
        mazosMejorados.Clear();
        oroActual = 100;
        PlayerPrefs.DeleteKey("ArmaduraEquipadaID");
        PlayerPrefs.DeleteKey("VidaExtraArmadura");
        vidaExtraArmadura = 0;
        Debug.Log("Inventario reseteado");
    }


}
