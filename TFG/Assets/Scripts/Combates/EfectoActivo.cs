// Archivo: EfectoActivo.cs (añadir soporte para daño en área)
using UnityEngine;

[System.Serializable]
public class EfectoActivo
{
    public string nombre;
    public TipoEfecto tipo;
    public int duracionTurnos;
    public int modificador;

    public void AplicarEfectoPorTurno(Luchador objetivo, Luchador lanzador = null)
    {
        duracionTurnos--;

        switch (tipo)
        {
            case TipoEfecto.Sangrado:
                objetivo.CambiarVida(-modificador);
                break;

            case TipoEfecto.Miedo:
                objetivo.bonusDaño -= modificador;
                break;

            case TipoEfecto.Furia:
                objetivo.bonusDaño += modificador;
                objetivo.estadoEspecial.FuriaRecibidaExtra = modificador;
                break;

            case TipoEfecto.Asqueado:
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
                if (lanzador != null)
                    lanzador.estadoEspecial.ReflejarDanioA = objetivo;
                break;

            case TipoEfecto.FuriaSanidad:
                float factor = 1f - Mathf.Clamp01(objetivo.sanidad / 10f);
                objetivo.bonusDaño += Mathf.CeilToInt(modificador * factor);
                break;

            case TipoEfecto.DañoEnArea:
                int dañoBase = modificador;
                if (lanzador != null && lanzador.estadoEspecial.FuriaRecibidaExtra > 0)
                {
                    dañoBase += lanzador.estadoEspecial.FuriaRecibidaExtra;
                    Debug.Log("Daño aumentado por Furia activa en lanzador.");
                }

                foreach (var luchador in Object.FindObjectsOfType<Luchador>())
                {
                    if (luchador.Aliado != objetivo.Aliado && luchador.sigueVivo)
                    {
                        luchador.CambiarVida(-dañoBase);
                        Debug.Log($"{luchador.nombre} recibe {dañoBase} de daño por área");
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

    public bool Expirado => duracionTurnos <= 0;
}