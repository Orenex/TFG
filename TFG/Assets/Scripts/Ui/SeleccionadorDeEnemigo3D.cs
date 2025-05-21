using UnityEngine;
using UnityEngine.EventSystems;

public class SeleccionadorDeEnemigo3D : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private LayerMask capaLuchadores;

    [Header("Indicador Visual")]
    [SerializeField] private GameObject indicadorSeleccionPrefab;

    private GameObject indicadorInstanciado;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SeleccionarConClick();
        }
    }

    private void SeleccionarConClick()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Clic ignorado sobre la UI.");
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, capaLuchadores))
        {
            Debug.Log($"Raycast HIT: {hit.collider.name}");

            Luchador luchador = hit.collider.GetComponentInParent<Luchador>();
            if (luchador != null && !luchador.Aliado && luchador.sigueVivo && luchador.gameObject.activeInHierarchy)
            {
                SeleccionDeObjetivo.Instance.SeleccionarObjetivo(luchador);
                MostrarIndicadorSobre(luchador.transform);
            }
            else
            {
                Debug.Log("Objeto clicado no es un enemigo válido.");
            }
        }
        else
        {
            Debug.Log("Raycast NO golpeó nada.");
        }
    }

    private void MostrarIndicadorSobre(Transform objetivo)
    {
        if (indicadorSeleccionPrefab == null)
        {
            Debug.LogWarning("Prefab del indicador no asignado.");
            return;
        }

        if (indicadorInstanciado == null)
        {
            indicadorInstanciado = Instantiate(indicadorSeleccionPrefab);
            indicadorInstanciado.layer = LayerMask.NameToLayer("Indicador");

            if (indicadorInstanciado.TryGetComponent<Collider>(out var col))
                Destroy(col); //elimina el collider completamente
        }

        indicadorInstanciado.transform.SetParent(null); // no hereda del objetivo
        indicadorInstanciado.transform.position = objetivo.position + Vector3.up * 2f;
        indicadorInstanciado.SetActive(true);
    }

    public void OcultarIndicador()
    {
        if (indicadorInstanciado != null)
        {
            indicadorInstanciado.SetActive(false);
        }
    }
}
