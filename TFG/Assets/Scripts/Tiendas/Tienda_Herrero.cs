using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

// Sistema de tienda del herrero para vender armas y armaduras
public class Tienda_Herrero : MonoBehaviour
{
    [System.Serializable]
    public class ObjetoTienda
    {
        public string nombre;     // Nombre del objeto
        public int precio;        // Precio en oro
        public Sprite icono;      // Icono para la interfaz
    }

    public GameObject panelItems;             // Panel que contiene los items
    public GameObject itemPrefab;             // Prefab para instanciar cada item
    public Transform contenidoScroll;         // Contenedor del scroll de items
    public TextMeshProUGUI goldText;          // Texto que muestra el oro actual
    public int gold = 100;                    // Oro del jugador

    public List<ObjetoTienda> armas;          // Lista de armas disponibles
    public List<ObjetoTienda> armaduras;      // Lista de armaduras disponibles

    // Muestra los objetos de la categoría seleccionada (armas o armaduras)
    public void MostrarCategoria(string categoria)
    {
        LimpiarScroll();
        List<ObjetoTienda> objetos = null;

        switch (categoria)
        {
            case "Armas": objetos = armas; break;
            case "Armaduras": objetos = armaduras; break;
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

    // Limpia los objetos del scroll actual
    void LimpiarScroll()
    {
        foreach (Transform hijo in contenidoScroll)
        {
            Destroy(hijo.gameObject);
        }
    }

    // Realiza la compra del objeto seleccionado
    void Comprar(ObjetoTienda obj)
    {
        if (gold >= obj.precio)
        {
            gold -= obj.precio;
            goldText.text = "Gold: " + gold;
            Debug.Log("Compraste: " + obj.nombre);
            // Aquí podrías añadir lógica para dar el objeto al jugador
        }
    }
}
