using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Me hago caca encima
/// </summary>
public class Deck : MonoBehaviour
{
    #region Fields and Properties

    public static Deck Instance { get; private set; } //Sigleton, significa que solo puede existir una unica instancia de esta clase

    //referenciamos las cartas que contiene el mazo

    /*aquí dice en el tutorial que se pueden añadir varios mazos, que podría servirnos para poner 4,
     uno para cada color, pero con el singleton no se si se podrían instanciar los cuatro a la vez, creo que el
     ejemplo que pone es en caso de tener la opcion de elegir entre varios mazos, pero luego solo usar uno de ellos a la vez*/
    [SerializeField] private CardCollection _redDeck;
    [SerializeField] private Carta _cartaPrefab; //el prefab de las cartas, del que haremos copias con la diferente informacion

    [SerializeField] private Canvas _cartaCanvas;


    //representacion de cartas instanciadas
    public List<Carta> _deckPila = new();
    public List<Carta> _descartePila = new();

    [field: SerializeField]public List<Carta> CartasMano { get; private set; } = new();


    #endregion

    #region Methods
    private void Awake()
    {
        //Declaración del Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        InstanciarDeck();
    }

    private void InstanciarDeck()
    {
        for (int i = 0; i < _redDeck.CartasEnLaColeccion.Count; i++)
        {
            Carta carta = Instantiate(_cartaPrefab, _cartaCanvas.transform); //instancia el Prefab de la carta como UI, hijo del Canvas de cartas
            carta.SetUp(_redDeck.CartasEnLaColeccion[i]);
            _deckPila.Add(carta); //al principio todas las cartas empiezan en el deck
            carta.gameObject.SetActive(false); //luego activaremos las cartas al robarlas
        }

        BarajarDeck();
    }


    //llamaremos esta funcion una vez al inicio, y luego cada vez que el mazo llegue a cero cartas
    private void BarajarDeck()
    {
        for(int i = _deckPila.Count-1; i >0; i--)
        {
            int j = Random.Range(0, i + 1);
            var temp = _deckPila[i];
            _deckPila[i] = _deckPila[j];
            _deckPila[j] = temp;
        }
    }


    public void DrawMano(int amount = 5)
    {
        for(int i = 0; i < amount; i++)
        {
            if(_deckPila.Count <= 0)
            {
                _descartePila = _deckPila;
                _descartePila.Clear();
                BarajarDeck();
            }

            CartasMano.Add(_deckPila[0]);
            _deckPila[0].gameObject.SetActive(true);
            _deckPila.RemoveAt(0);
        }
    }


    public void DescartarCarta(Carta carta)
    {
        
        if (CartasMano.Contains(carta))
        {
            CartasMano.Remove(carta);
            _descartePila.Add(carta);
            carta.gameObject.SetActive(false);
        }
        
    }



    #endregion

}
