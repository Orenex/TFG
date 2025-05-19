using UnityEngine;

[CreateAssetMenu(fileName = "NuevaAccion", menuName = "Cartas/Accion")]
public class CartasAccion : ScriptableObject
{
    [Header("Datos de la acci�n")]
    public int da�o;

    /// <summary>
    /// Aplica la acci�n a un objetivo desde un lanzador.
    /// </summary>
    public virtual void Aplicar(Luchador lanzador, Luchador objetivo)
    {
        if (objetivo == null)
        {
            Debug.LogWarning("No hay objetivo v�lido.");
            return;
        }

        if (!objetivo.sigueVivo)
        {
            Debug.LogWarning("El objetivo ya est� derrotado.");
            return;
        }

        objetivo.vida -= da�o;
        Debug.Log($"{lanzador.name} infligi� {da�o} da�o a {objetivo.name}.");

        if (objetivo.vida <= 0)
        {
            objetivo.sigueVivo = false;
            Debug.Log($"{objetivo.name} ha sido derrotado.");
        }
    }
}
