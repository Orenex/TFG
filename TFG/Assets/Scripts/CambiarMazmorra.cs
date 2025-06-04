using UnityEngine;

public class CambiarMazmorra : MonoBehaviour
{
    public GameObject Catacumbas;
    public GameObject Sectarios;
    void Start()
    {
        Catacumbas.SetActive(false);
        Sectarios.SetActive(false);
    }

    
    void Update()
    {
        CambiarZona();
    }
    private void CambiarZona()
    {
        if (ConfirmarFinCatacumbas.mazmorra1Completada == true)
        {
            Catacumbas.SetActive(false);
            Sectarios.SetActive(true);
        }
        else
        {
            Catacumbas.SetActive(true);
            Sectarios.SetActive(false);
        }
    }
}
