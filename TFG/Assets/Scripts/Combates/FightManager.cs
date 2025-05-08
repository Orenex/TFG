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

    //usare el script de la mano para pedir prestada la lista de las cartas presentes en el momento, y así sacar sus botones
    public Mano scriptMano;
    public GameObject[] botonesCartas;


    #endregion

    #region Methods
    void Awake()
    {
        //otra declaracion del singleton, más compleja
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
                    if (luchador.aliado)
                    {
                        foreach (var accion in luchador.Acciones)
                        {
                            Button b = null;
                            for (int i = 0; i < poolBotones.Count; i++)
                            {
                                Debug.Log("HOALAA");
                                if (!poolBotones[i].gameObject.activeInHierarchy)
                                {
                                    b = botonesCartas[i].GetComponent<Button>();
                                }
                            }

                            b = GetComponentInChildren<Button>();


                            poolBotones.Add(b);

                            b.gameObject.SetActive(true);
                            b.onClick.RemoveAllListeners();
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
                        luchador.EjecutarAccion(luchador.Acciones[Random.Range(0, luchador.Acciones.Count)], luchadores[Random.Range(0, luchadores.Count)].transform);
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
