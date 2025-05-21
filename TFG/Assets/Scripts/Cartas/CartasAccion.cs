using UnityEngine;

/// <summary>
/// ScriptableObject que representa una acción ejecutable por una carta.
/// </summary>
[CreateAssetMenu(fileName = "NuevaAccion", menuName = "Cartas/Accion")]
public class CartasAccion : ScriptableObject
{
    [Header("Datos de la acción")]
    public int daño;

    /// <summary>
    /// Aplica la acción a un objetivo desde un lanzador.
    /// </summary>
    public virtual void Aplicar(Luchador lanzador, Luchador objetivo)
    {
        if (objetivo == null)
        {
            Debug.LogWarning("No hay objetivo válido.");
            return;
        }

        if (!objetivo.sigueVivo)
        {
            Debug.LogWarning("El objetivo ya está derrotado.");
            return;
        }

        objetivo.CambiarVida(-daño);
        Debug.Log($"{lanzador.name} infligió {daño} daño a {objetivo.name}.");

        if (objetivo.vida <= 0)
        {
            objetivo.sigueVivo = false;
            Debug.Log($"{objetivo.name} ha sido derrotado.");
        }
    }
}
