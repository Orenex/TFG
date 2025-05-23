// Archivo: EfectoActivo.cs (a�adir soporte para da�o en �rea)
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
                objetivo.bonusDa�o -= modificador;
                break;

            case TipoEfecto.Furia:
                objetivo.bonusDa�o += modificador;
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
                    objetivo.furiaFocalizada.bonusDa�o = modificador;
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

            case TipoEfecto.CompartirDa�o:
                if (lanzador != null)
                    lanzador.estadoEspecial.ReflejarDanioA = objetivo;
                break;

            case TipoEfecto.FuriaSanidad:
                float factor = 1f - Mathf.Clamp01(objetivo.sanidad / 10f);
                objetivo.bonusDa�o += Mathf.CeilToInt(modificador * factor);
                break;

            case TipoEfecto.Da�oEnArea:
                int da�oBase = modificador;
                if (lanzador != null && lanzador.estadoEspecial.FuriaRecibidaExtra > 0)
                {
                    da�oBase += lanzador.estadoEspecial.FuriaRecibidaExtra;
                    Debug.Log("Da�o aumentado por Furia activa en lanzador.");
                }

                foreach (var luchador in Object.FindObjectsOfType<Luchador>())
                {
                    if (luchador.Aliado != objetivo.Aliado && luchador.sigueVivo)
                    {
                        luchador.CambiarVida(-da�oBase);
                        Debug.Log($"{luchador.nombre} recibe {da�oBase} de da�o por �rea");
                    }
                }
                break;
        }
    }

    public class FuriaFocalizada
    {
        public Luchador objetivo;
        public int bonusDa�o;
        public int turnosRestantes;
    }

    public bool Expirado => duracionTurnos <= 0;
}