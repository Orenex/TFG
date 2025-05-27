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

    public GameObject panelItems;
    public GameObject itemPrefab;
    public Transform contenidoScroll;
    public TextMeshProUGUI goldText;
    public int gold = 100;

    public List<ObjetoTienda> mejorar;

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

    void LimpiarScroll()
    {
        foreach (Transform hijo in contenidoScroll)
        {
            Destroy(hijo.gameObject);
        }
    }

    void Comprar(ObjetoTienda obj)
    {
        if (gold >= obj.precio)
        {
            gold -= obj.precio;
            goldText.text = "Gold: " + gold;
            Debug.Log("Compraste: " + obj.nombre);
        }
    }
}
