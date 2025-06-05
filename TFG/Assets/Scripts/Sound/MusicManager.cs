using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    private AudioSource AuSource;

    public AudioClip[] musica;

    private Scene currentEscena;

    private Scene anteriorEscena;

    private bool dontStop;


    void Start()
    {
        DontDestroyOnLoad(gameObject);

        AuSource = gameObject.GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        currentEscena = SceneManager.GetActiveScene();

        if(currentEscena != anteriorEscena && dontStop == false)
        {
            AuSource.Stop();
        }

        switch (currentEscena.name)
        {
            case "Menu_Inicio":
            case "Entrada_Catacumbas":
                changeMusic(0);
                break;
            case "Historia":
                changeMusic(1);
                break;
            case "PuebloPrincipal":
            case "Introduccion_Marceau":
                changeMusic(2);
                break;
            case "TiendaBruja":
            case "TiendaBruja_Mejor":
            case "TiendaHerrero":
                changeMusic(3);
                break;
            case "MapaCatacumbas":
            case "Combate_Catacumbas_GlenSolo":
            case "Combate_Catacumbas_Equipo":
            case "CofreCatacumbas":

                //necesito que no se resetee la musica entre estas escenas
                //primero cambiamos musica y hacemos dontstop true, que evitara que
                //cambie la musica siempre y cuando la escena que se carga no tenga 
                //el statemente dontstop = false al principio
                anteriorEscena = SceneManager.GetActiveScene();

                if (AuSource.isPlaying == false)
                {
                    AuSource.resource = musica[4];
                    AuSource.Play();
                }

                dontStop = true;

                break;
            case "Descanso_Catacumbas":
            case "Descanso_Sectarios":
                changeMusic(5);
                break;
            case "Introduccion_Jack":
                changeMusic(6);
                break;
            case "Boss_Catacumbas":
                changeMusic(7);
                break;
            case "FinalMazmorraCatacumbas":
                changeMusic(8);
                break;
            case "Entrada_Sectarios":
                changeMusic(9);
                break;
            case "MapaPuebloSectario":
            case "Combate_Sectarios_Zona_01":
            case "Combate_Sectarios_Zona_02":
            case "CofreSectarios":

                //necesito que no se resetee la musica entre estas escenas
                //primero cambiamos musica y hacemos dontstop true, que evitara que
                //cambie la musica siempre y cuando la escena que se carga no tenga 
                //el statemente dontstop = false al principio
                anteriorEscena = SceneManager.GetActiveScene();

                if (AuSource.isPlaying == false)
                {
                    AuSource.resource = musica[10];
                    AuSource.Play();
                }

                dontStop = true;
                break;
            case "Boss_Sectarios":
                changeMusic(11);
                break;
            case "GameOver":
                changeMusic(13);
                break;



        }

        void changeMusic(int musicNum)
        {
            if(dontStop == true)
            {
                dontStop = false;
                AuSource.Stop();
            }
            anteriorEscena = SceneManager.GetActiveScene();

            if (AuSource.isPlaying == false)
            {
                AuSource.resource = musica[musicNum];
                AuSource.Play();
            }
        }

    }
}
