using UnityEngine;

public class CambiarMazmorra : MonoBehaviour
{
    public GameObject Catacumbas;
    public GameObject Sectarios;
    public GameObject BrujaNormal;
    public GameObject BrujaMejorada;
    void Start()
    {
        Catacumbas.SetActive(false);
        Sectarios.SetActive(false);
        BrujaNormal.SetActive(false);
        BrujaMejorada.SetActive(false);
    }

    
    void Update()
    {
        CambiarZona();
    }
    private void CambiarZona()
    {
        if (ConfirmarFinCatacumbas.mazmorra1Completada == true)
        {
            // Si la primera mazmorra fue completada, activa los nuevos enemigos y zonas
            Catacumbas.SetActive(false);
            Sectarios.SetActive(true);
            BrujaNormal.SetActive(false);
            BrujaMejorada.SetActive(true);
        }
        else
        {
            // Si no fue completada, se mantiene en la primera zona
            Catacumbas.SetActive(true);
            Sectarios.SetActive(false);
            BrujaNormal.SetActive(true);
            BrujaMejorada.SetActive(false);
        }
    }
}
