using UnityEngine;

public class SeleccionadorDeEnemigo3D : MonoBehaviour
{
    [SerializeField] private LayerMask layerEnemigos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayo, out RaycastHit hit, 100f, layerEnemigos))
            {
                var luchador = hit.collider.GetComponentInParent<Luchador>();
                if (luchador != null && luchador.sigueVivo && luchador.Aliado == false)
                {
                    SeleccionDeObjetivo.Instance.SeleccionarObjetivo(luchador);
                }
            }
        }
    }
}
