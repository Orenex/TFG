using UnityEngine;

public class CardActionExecutor : MonoBehaviour
{
    public static CardActionExecutor Instance { get; private set; }

    [Header("Referencia de combate")]
    [SerializeField] private FightManager fightManager;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        if (fightManager == null)
            fightManager = FindObjectOfType<FightManager>();
    }

    /// <summary>
    /// Ejecuta la acci�n de la carta sobre el objetivo.
    /// </summary>
    public void JugarCarta(Carta carta, Luchador objetivo)
    {
        if (carta == null || objetivo == null)
        {
            Debug.LogWarning("Carta u objetivo inv�lido.");
            return;
        }

        Accion accion = carta.DataCarta.accion;
        Luchador lanzador = fightManager.GetLuchadorActual();

        if (lanzador.mana < accion.costoMana)
        {
            Debug.Log("�No tienes suficiente man� para usar esta carta!");
            return;
        }

        fightManager.EjecutarAccionJugador(accion, objetivo);
        carta.gameObject.SetActive(false); // se descarta visualmente
    }
}
