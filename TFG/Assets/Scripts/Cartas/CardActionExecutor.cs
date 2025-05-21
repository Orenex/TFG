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
    /// Ejecuta la acción de la carta sobre el objetivo seleccionado.
    /// </summary>
    public void JugarCarta(Carta carta, Luchador objetivo)
    {
        if (carta == null || objetivo == null)
        {
            Debug.LogWarning("CardActionExecutor: Carta u objetivo inválido.");
            return;
        }

        var tipo = carta.DataCarta.tipo;

        // Carta duplicadora
        if (tipo == TipoCarta.Duplicadora)
        {
            var cartaADuplicar = MovimientoCarta.ObtenerCartaSeleccionada();

            if (cartaADuplicar == null || cartaADuplicar == carta)
            {
                Debug.LogWarning("CardActionExecutor: Selecciona otra carta válida para duplicar.");
                return;
            }

            var duplicadoGO = Instantiate(cartaADuplicar.gameObject, cartaADuplicar.transform.parent);
            var duplicado = duplicadoGO.GetComponent<Carta>();

            if (duplicado == null)
            {
                Debug.LogError("CardActionExecutor: No se pudo obtener componente 'Carta' del duplicado.");
                Destroy(duplicadoGO);
                return;
            }

            duplicado.SetUp(cartaADuplicar.DataCarta);
            duplicado.gameObject.SetActive(true);

            Transform ancla = fightManager.deck.mano.ObtenerSiguienteAncla(out int index);
            if (ancla != null)
            {
                duplicado.transform.SetParent(ancla, false);
                duplicado.transform.localPosition = Vector3.zero;
                duplicado.transform.localRotation = Quaternion.identity;
                duplicado.indiceAncla = index;
                fightManager.deck.CartasEnMano.Add(duplicado);
            }
            else
            {
                Debug.LogWarning("CardActionExecutor: No hay espacio para duplicar la carta.");
                Destroy(duplicado.gameObject);
            }

            fightManager.deck.DescartarCarta(carta);
            fightManager.manoJugador.LiberarPosicion(carta.indiceAncla);
            carta.gameObject.SetActive(false);
            fightManager.TerminarTurno();
            return;
        }

        // Acción normal
        Accion accion = carta.DataCarta.accion;
        Luchador lanzador = fightManager.GetLuchadorActual();

        bool tieneRecursos = accion.tipoCoste switch
        {
            RecursoCoste.Mana => lanzador.mana >= accion.costoMana,
            RecursoCoste.Sanidad => lanzador.sanidad >= accion.costoMana,
            _ => false
        };

        if (!tieneRecursos)
        {
            Debug.Log("CardActionExecutor: ¡No tienes recursos suficientes para usar esta carta!");
            return;
        }

        fightManager.EjecutarAccionJugador(accion, objetivo);

        fightManager.deck.DescartarCarta(carta);
        fightManager.manoJugador.LiberarPosicion(carta.indiceAncla);
        carta.gameObject.SetActive(false);
    }
}
