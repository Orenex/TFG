using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Este Script va en el CardCanvas
/// </summary>
public class Mano : MonoBehaviour
{
    #region Fields and Properties

    //la variable anclas determina lo grande que es la mano (por ahora nos comformamos con 5)
    public static GameObject[] _anclas;

    //la var posLibre es una buleana que indica si la posicion esta siendo ocupada, en cuyo caso no permitirá otra carta en la misma pos
    [SerializeField] public static bool[] posLibre;

    private readonly string ANCLA_MANO = "AnclaMano";


    //toggle para descartar cartas
    public static bool isDiscarting;
    public GameObject _botonConfirm; //boton de confirmar descarte que esconderemos cuando no estemos descartando

    //pillamos las propias cartas desde el scrip de movimiento (cuya funcion es la que llamaremos)
    [SerializeField]private GameObject[] _cardGO;
    public MovimientoCarta[] _card; 
    private readonly string CARTAS = "Carta";
    #endregion


    #region Methods
    private void Awake()
    {     
        _anclas = GameObject.FindGameObjectsWithTag(ANCLA_MANO);       

        posLibre = new bool[_anclas.Length];

        //nada más crear el mazo establezco todos los espacios de carta libres
        for (int i = 0; i <_anclas.Length; i++)
        {
           posLibre[i] = true;
        }
    }

    public void IsDiscarting()
    {
        isDiscarting = !isDiscarting;
        Debug.Log("isdiscarting = " + isDiscarting);

        _card = new MovimientoCarta[_cardGO.Length];

        for (int i = 0; i <_cardGO.Length; i++)
        {
            _card[i] = _cardGO[i].GetComponent<MovimientoCarta>();
        }
    }

    public void DescartarSeleccion() //funcion que tendra el boton de confirmar descarte para descartar todas las cartas al unisono
    {
        for (int i = 0; i < _cardGO.Length; i++)
        {
            _card[i].PuedoDescartar();
        }
        isDiscarting = false;
    }

    private void Update()
    {
        if(isDiscarting == true)
        {
            _botonConfirm.SetActive(true);
            
        }
        else
        {
            _botonConfirm.SetActive(false);
        }

        if(_cardGO != null)
        {
            _cardGO = GameObject.FindGameObjectsWithTag(CARTAS);
        }
        
    }
    #endregion
}
