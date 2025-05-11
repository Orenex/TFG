using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    #region Fields and Properties

    public List<Luchador> luchadores;

    public static FightManager singleton;


    public Button prefab;
    public Text textoEstado;
    public Transform panel;

    //usare el script de la mano para pedir prestada la lista de las cartas presentes en el momento, y as� sacar sus botones
    public Mano scriptMano;
    public GameObject[] botonesCartas;


    #endregion

    #region Methods
    void Awake()
    {
        //otra declaracion del singleton, m�s compleja
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
    }

    private void Start()
    {
        StartCoroutine("Bucle");
    }

    List<Button> poolBotones = new List<Button>();

    IEnumerator Bucle()
    {
        while (true)
        {
            foreach (var luchador in luchadores)
            {

                IEnumerator c = null;

                for (int i = 0; i < poolBotones.Count; i++)
                {
                    poolBotones[i].gameObject.SetActive(false);
                }

                if (luchador.sigueVivo)
                {
                    //TODA ESTA PARTE ES SUSCEPTIBLE A CAMBIO, PORQUE ESTO LO HACE EL TIO DLE TUTORIAL BAJO LA REALIDAD DE QUE UN MISMO BOTON
                    //SERVIRA PARA VARIOS ATAQUES DEPENDIENDO DEL PERSONAJE ACTIVO, PERO EN NUESTRO CASO NO ES ASI, CADA CARTA TIENE UN BOTON
                    //ESPECIFICO QUE EJECUTAR� AL SER CLICADO LA ACCION ESPEC�FICA DEL PERSONAJE ASI QUE NO SE SI HABRIA QUE A�ADIR LOS LISTENERS
                    //DURANTE EL PROPIO JUEGO O SE PODR�A TENER HECHO DE ANTES (SUPONGO QUE NO PORQUE ANTES DE EJECUTAR LA ESCENA EL MAZO NO EXISTE AUN
                    //A VER COMO HACEMOS ESO
                    if (luchador.aliado)
                    {
                        foreach (var cardGO in scriptMano._cardGO)
                        {
                            Button b = GetComponent<Button>();
                            poolBotones.Add(b);

                            cardGO.GetComponentInChildren<Text>();

                            for (int i = 0; i < poolBotones.Count; i++)
                            {


                                Debug.Log("HOALAA");
                                if (cardGO. == "Ataque")
                                {
                                    b = poolBotones[i];
                                }
                            }

                            b = botonesCartas[1].GetComponent<Button>();


                            

                            b.gameObject.SetActive(true);
                            if (luchador.mana < accion.costoMana)
                            {
                                b.interactable = false;
                            }
                            else
                            {
                                b.interactable = true;
                                b.onClick.AddListener(() =>
                                {
                                    for (int j = 0; j < poolBotones.Count; j++)
                                    {
                                        poolBotones[j].gameObject.SetActive(false);
                                    }

                                    c = luchador.EjecutarAccion(accion, luchadores[Random.Range(0, luchadores.Count)].transform);

                                });
                            }
                        }
                    }
                    else
                    {
                        Debug.Log(luchador);
                        luchador.MatameCojones(luchador.Acciones[0], luchadores[1].gameObject);
                        
                    }

                    while (c == null)
                    {
                        yield return null;
                    }
                    StartCoroutine(c);
                }
            }
        }
    }

    void Update()
    {
        botonesCartas = scriptMano._cardGO;
    }
    #endregion
}
