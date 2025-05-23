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
                // Reduce el daño que inflige el objetivo
                objetivo.bonusDaño -= modificador;
                break;

            case TipoEfecto.Furia:
                // Aumenta daño infligido, pero también el recibido
                objetivo.bonusDaño += modificador;
                objetivo.estadoEspecial.FuriaRecibidaExtra = modificador;
                break;

            case TipoEfecto.Asqueado:
                objetivo.estadoEspecial.Asqueado = true;
                break;

            case TipoEfecto.FuriaFocalizada:
                // Solo si no está ya activa, se aplica
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
                lanzador.estadoEspecial.ReflejarDanioA = objetivo;
                break;
           
            case TipoEfecto.FuriaSanidad:
                float factor = 1f - Mathf.Clamp01(objetivo.sanidad / 10f);
                objetivo.bonusDaño += Mathf.CeilToInt(modificador * factor);
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
