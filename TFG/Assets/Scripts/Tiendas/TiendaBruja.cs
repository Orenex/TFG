using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Sistema de tienda de la bruja para mejoras
public class TiendaBruja : MonoBehaviour
{
    [System.Serializable]
    public class ObjetoTienda
    {
        public string nombre;     // Nombre del objeto de mejora
        public int precio;        // Precio en oro
        public Sprite icono;      // Icono mostrado en la interfaz
    }

    public GameObject panelItems;             // Panel de la tienda
    public GameObject itemPrefab;             // Prefab de item en UI
    public Transform contenidoScroll;         // Contenedor del scroll
    public TextMeshProUGUI goldText;          // Texto con el oro actual
    public int gold = 100;                    // Oro inicial del jugador

    public List<ObjetoTienda> mejorar;        // Lista de mejoras disponibles

    // Muestra los objetos disponibles según la categoría
    public void MostrarCategoria(string categoria)
    {
        LimpiarScroll();
        List<ObjetoTienda> objetos = null;

        switch (categoria)
        {
            case "Mejorar": objetos = mejorar; break;
        }

        foreach (var obj in objetos)
        {
            GameObject nuevo = Instantiate(itemPrefab, contenidoScroll);
            nuevo.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = obj.nombre;
            nuevo.transform.Find("Precio").GetComponent<TextMeshProUGUI>().text = obj.precio + " G";
            nuevo.transform.Find("Icono").GetComponent<Image>().sprite = obj.icono;

            Button boton = nuevo.transform.Find("BotonComprar").GetComponent<Button>();
            boton.onClick.AddListener(() => Comprar(obj));
        }

        panelItems.SetActive(true);
    }

    // Limpia los objetos anteriores del scroll
    void LimpiarScroll()
    {
        foreach (Transform hijo in contenidoScroll)
        {
            Destroy(hijo.gameObject);
        }
    }

    // Realiza la compra del objeto si hay suficiente oro
    void Comprar(ObjetoTienda obj)
    {
        if (gold >= obj.precio)
        {
            gold -= obj.precio;
            goldText.text = "Gold: " + gold;
            Debug.Log("Compraste: " + obj.nombre);
            // Aquí podrías aplicar los efectos de mejora
        }
    }
}
