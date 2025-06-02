using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Tienda_Herrero : MonoBehaviour
{
    [System.Serializable]
    public class ArmaduraTienda
    {
        public string id;
        public string nombre;
        public int precio;
        public int vidaExtra;
        public Sprite icono;
    }

    public GameObject panelItems;
    public GameObject itemPrefab;
    public Transform contenidoScroll;
    public TextMeshProUGUI goldText;

    public List<ArmaduraTienda> armaduras;

    private void Start()
    {
        goldText.text = "Gold: " + InventarioJugador.Instance.ObtenerOro();
        MostrarArmaduras();
    }

    public void MostrarArmaduras()
    {
        LimpiarScroll();

        foreach (var armadura in armaduras)
        {
            GameObject nuevo = Instantiate(itemPrefab, contenidoScroll);
            nuevo.transform.Find("Nombre").GetComponent<TextMeshProUGUI>().text = armadura.nombre;
            nuevo.transform.Find("Precio").GetComponent<TextMeshProUGUI>().text = armadura.precio + " G";
            nuevo.transform.Find("Icono").GetComponent<Image>().sprite = armadura.icono;

            Button boton = nuevo.transform.Find("BotonComprar").GetComponent<Button>();
            TextMeshProUGUI textoBoton = boton.GetComponentInChildren<TextMeshProUGUI>();

            if (InventarioJugador.Instance.EsArmaduraComprada(armadura.id))
            {
                textoBoton.text = "Comprado";
                boton.interactable = false;
            }
            else
            {
                textoBoton.text = "Comprar";
                boton.onClick.AddListener(() =>
                {
                    ComprarArmadura(armadura);
                    MostrarArmaduras();
                });
            }
        }

        panelItems.SetActive(true);
    }

    void LimpiarScroll()
    {
        foreach (Transform hijo in contenidoScroll)
            Destroy(hijo.gameObject);
    }

    void ComprarArmadura(ArmaduraTienda armadura)
    {
        if (!InventarioJugador.Instance.TieneOro(armadura.precio)) return;

        InventarioJugador.Instance.GastarOro(armadura.precio);
        InventarioJugador.Instance.GuardarArmaduraComprada(armadura.id, armadura.vidaExtra);
        goldText.text = "Gold: " + InventarioJugador.Instance.ObtenerOro();
        Debug.Log("Compraste armadura: " + armadura.nombre);
    }
}
