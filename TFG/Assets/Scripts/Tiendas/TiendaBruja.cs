using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TiendaBruja : MonoBehaviour
{
    [System.Serializable]
    public class ObjetoTienda
    {
        public string nombre;
        public int precio;
        public Sprite icono;
    }

    [System.Serializable]
    public class MazoTienda
    {
        public string id;
        public string nombre;
        public int precio;
        public Sprite icono;
        public CardCollection mazoBase;
        public CardCollection mazoMejorado;
    }

    public GameObject panelItems;
    public GameObject itemPrefab;
    public Transform contenidoScroll;
    public TextMeshProUGUI goldText;
    public List<MazoTienda> mazosDisponibles;

    private void Start()
    {
        goldText.text = "Gold: " + InventarioJugador.Instance.ObtenerOro();
    }

    public void MostrarCategoria(string categoria)
    {
        LimpiarScroll();

        switch (categoria)
        {

            case "Mazos":
                foreach (var mazo in mazosDisponibles)
                {
                    Debug.Log($"Cargando mazo: {mazo.nombre}, ID='{mazo.id}'");

                    GameObject nuevo = Instantiate(itemPrefab, contenidoScroll);
                    nuevo.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = mazo.nombre;
                    nuevo.transform.Find("Precio").GetComponent<TextMeshProUGUI>().text = mazo.precio + " G";
                    nuevo.transform.Find("Icono").GetComponent<Image>().sprite = mazo.icono;

                    Button boton = nuevo.transform.Find("BotonComprar").GetComponent<Button>();
                    TextMeshProUGUI textoBoton = boton.GetComponentInChildren<TextMeshProUGUI>();

                    if (InventarioJugador.Instance.EsMazoMejorado(mazo.id))
                    {
                        textoBoton.text = "Comprado";
                        boton.interactable = false;
                        Debug.Log("UI: Mazo ya comprado " + mazo.id);
                    }
                    else
                    {
                        textoBoton.text = "Comprar";
                        boton.onClick.AddListener(() =>
                        {
                            ComprarMazo(mazo);
                            Invoke(nameof(RefrescarMazos), 0.1f); // pequeña pausa
                        });
                    }
                }
                break;
        }

        panelItems.SetActive(true);
    }

    void RefrescarMazos()
    {
        MostrarCategoria("Mazos");
    }

    void LimpiarScroll()
    {
        foreach (Transform hijo in contenidoScroll)
        {
            Destroy(hijo.gameObject);
        }
    }

    void Comprar(ObjetoTienda obj)
    {
        if (InventarioJugador.Instance.TieneOro(obj.precio))
        {
            InventarioJugador.Instance.GastarOro(obj.precio);
            goldText.text = "Gold: " + InventarioJugador.Instance.ObtenerOro();
        }
    }

    void ComprarMazo(MazoTienda mazo)
    {
        Debug.Log("ComprarMazo fue llamado para ID: " + mazo.id);

        if (InventarioJugador.Instance.TieneOro(mazo.precio))
        {
            InventarioJugador.Instance.GastarOro(mazo.precio);
            InventarioJugador.Instance.MarcarMazoComoMejorado(mazo.id);
            goldText.text = "Gold: " + InventarioJugador.Instance.ObtenerOro();
            Debug.Log($"Has comprado el mazo mejorado: {mazo.nombre}");
            InventarioJugador.Instance.DebugMazosComprados();
        }
        else
        {
            Debug.LogWarning("No tienes suficiente oro para comprar: " + mazo.nombre);
        }
    }
}
