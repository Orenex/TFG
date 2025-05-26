// Clase que representa un efecto aplicado a un luchador y su comportamiento por turno
using UnityEngine;

[System.Serializable]
public class EfectoActivo
{
    public string nombre;
    public TipoEfecto tipo;
    public int duracionTurnos;
    public int modificador;

    public Luchador lanzador;

    public void AplicarEfectoPorTurno(Luchador objetivo, Luchador lanzadorIgnorado = null)
    {
        if (tipo != TipoEfecto.Sangrado)
            duracionTurnos--;

        switch (tipo)
        {
            case TipoEfecto.Sangrado:
                Debug.Log($"{objetivo.nombre} sufre sangrado permanente.");
                objetivo.estadoEspecial.Sangrado = true;
                break;

            case TipoEfecto.Furia:
                objetivo.bonusDaño += modificador;
                objetivo.estadoEspecial.FuriaRecibidaExtra = modificador;
                break;

            case TipoEfecto.Asqueado:
                Debug.Log($"{objetivo.nombre} vomita del asco.");
                objetivo.estadoEspecial.Asqueado = true;
                break;

            case TipoEfecto.FuriaFocalizada:
                if (objetivo.furiaFocalizada == null)
                {
                    objetivo.furiaFocalizada = new FuriaFocalizada();
                    objetivo.furiaFocalizada.objetivo = SeleccionDeObjetivo.Instance.ObtenerObjetivoActual();
                    objetivo.furiaFocalizada.bonusDaño = modificador;
                    objetivo.furiaFocalizada.turnosRestantes = 3;
                }
                break;

            case TipoEfecto.Paralizado:
                objetivo.estadoEspecial.Paralizado = true;
                break;

            case TipoEfecto.Critico:
                objetivo.estadoEspecial.Critico = true;
                break;

            case TipoEfecto.CondicionPerfecta:
                objetivo.estadoEspecial.CondicionPerfecta = true;
                break;

            case TipoEfecto.ResucitarUnaVez:
                objetivo.estadoEspecial.ResucitarUnaVez = true;
                break;

            case TipoEfecto.Confusion:
                objetivo.estadoEspecial.Confusion = true;
                break;

            case TipoEfecto.CompartirDaño:
                if (lanzador != null && lanzador != objetivo)
                {
                    lanzador.estadoEspecial.ReflejarDanioA = objetivo;
                    Debug.Log($"{lanzador.nombre} marcará a {objetivo.nombre} para recibir el daño que él reciba.");
                }
                break;

            case TipoEfecto.FuriaSanidad:
                float factor = 1f - Mathf.Clamp01(objetivo.sanidad / 10f);
                objetivo.bonusDaño += Mathf.CeilToInt(modificador * factor);
                break;

            case TipoEfecto.DanioEnArea:
                foreach (var luchador in Object.FindObjectsOfType<Luchador>())
                {
                    if (luchador.Aliado != lanzador.Aliado && luchador.sigueVivo)
                    {
                        luchador.CambiarVida(modificador * -1);
                        Debug.Log($"{luchador.nombre} recibe daño de área: {modificador} HP.");
                    }
                }
                break;
        }
    }

    public class FuriaFocalizada
    {
        public Luchador objetivo;
        public int bonusDaño;
        public int turnosRestantes;
    }

    public bool Expirado => tipo != TipoEfecto.Sangrado && duracionTurnos <= 0;
}
