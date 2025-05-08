using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovimientoCarta : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler //estos tres son necesarios para que la carta detecte el raton, y ademas yo me la quiero cortar porque llevo 3 horas pa esta mierda
{
    #region Fields and Properties
    private bool _isSelected;
    private Canvas _cardCanvas;
    private RectTransform _rectTransform;
    private Carta _card;

    private readonly string CANVAS_TAG = "CardCanvas";

    //posicionar cartas
    
    [SerializeField] private RectTransform[] _posAnclas;

    //tamaño que una carta crece y decrece al ser seleccionada
    public Vector3 crecimientoAlSeleccionar;

    //se registrará en que posicion esta la carta
    [SerializeField] private int _miPosicion;

    #endregion


    # region Methods

    private void Start()
    {
        _cardCanvas = GameObject.FindGameObjectWithTag(CANVAS_TAG).GetComponent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _card = GetComponent<Carta>();


        //saco el recttransform de los espacios de cartas con el array
     
        _posAnclas = new RectTransform[Mano._anclas.Length];

        for (int i = 0; i < Mano._anclas.Length; i++)
        {
            _posAnclas[i] = Mano._anclas[i].GetComponent<RectTransform>();
        }

        for (int i = 0; i < Mano._anclas.Length; i++)
        {
            if (Mano.posLibre[i] == true)
            {
                _rectTransform.position = _posAnclas[i].position;
                _rectTransform.rotation = _posAnclas[i].rotation;
                _rectTransform.localScale = _posAnclas[i].localScale;
                Mano.posLibre[i] = false;
                _miPosicion = i;
                break; //el break hace que el loop se detenga en caso de encontrar un espacio libre, sin esto la misma carta viajaría por cada uno de los espacios ocupándolos todos
            }
            else
            {

            }
        }

    }

    private void OnEnable()
    {
        if(_cardCanvas != null) //el enable no hará nada la primera vez, una vez se ejecute el start (y por lo tanto el cardcanvas no sea null) es que será libre de coger posicion a cada activacion del objeto
        {
            for (int i = 0; i < Mano._anclas.Length; i++)
            {
                if (Mano.posLibre[i] == true)
                {
                    _rectTransform.position = _posAnclas[i].position;
                    _rectTransform.rotation = _posAnclas[i].rotation;
                    Mano.posLibre[i] = false;
                    _miPosicion = i;
                    break; //el break hace que el loop se detenga en caso de encontrar un espacio libre, sin esto la misma carta viajaría por cada uno de los espacios ocupándolos todos
                }
                else
                {

                }
            }
        }
    }

    //detectar raton
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(_isSelected == false)
        {
            _rectTransform.localScale += crecimientoAlSeleccionar;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_isSelected == false)
        {
            _rectTransform.localScale += -crecimientoAlSeleccionar;
        }
        
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (Mano.isDiscarting == true)
        {
            if(_isSelected == false)
            {
                _isSelected = true;
            }
            else
            {
                _isSelected = false;
            }
        }
    }

    //al pulsar el boton de confirmar descarte se activará esta función, que llamara a la funcion de descarte del deck en caso de estar seleccionada
    public void PuedoDescartar()
    {
        if (_isSelected == true)
        {
            Deck.Instance.DescartarCarta(_card);
            Debug.Log("Posicion " + _miPosicion + " esta libre");
            Mano.posLibre[_miPosicion] = true; //vuelvo a establecer que la posicion de esta carta esta libre
        }
        else
        {

        }
    }

    private void Update()
    {
        /*detectamos si se desactiva el toggle para descartar, automaticamente deseleccionamos todas las cartas al hacerlo
         reiniciando tambien su tamaño para que sea el original (solo en caso de haber estado seleccionadas)*/
        if(Mano.isDiscarting == false)
        {
            if(_isSelected == true)
            {
                _isSelected = false;
                _rectTransform.localScale += -crecimientoAlSeleccionar;
            }
            
        }
    }

    #endregion
}
