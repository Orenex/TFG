using UnityEngine;

[System.Serializable]
public class EfectoActivo
{
    public string nombre;
    public TipoEfecto tipo;
    public int duracionTurnos;
    public int modificador;
    private bool yaActivado = false;

    public void AplicarEfectoPorTurno(Luchador objetivo)
    {
        switch (tipo)
        {
            case TipoEfecto.Sangrado:
                objetivo.CambiarVida(-modificador);
                break;

            case TipoEfecto.CurarPorTurno:
                objetivo.CambiarVida(modificador);
                break;

            case TipoEfecto.Asqueado:
                objetivo.estadoEspecial.Asqueado = true;
                break;

            case TipoEfecto.Furia:
                objetivo.bonusDa�o += modificador;
                break;

            case TipoEfecto.ReducirDa�o:
                objetivo.bonusDa�o -= modificador;
                break;

            case TipoEfecto.RobarSalud:
                var enemigos = objetivo.ObtenerEnemigosCercanos();
                foreach (var e in enemigos)
                {
                    e.CambiarVida(-modificador);
                    objetivo.CambiarVida(modificador);
                }
                break;

            case TipoEfecto.CompartirDa�o:
                objetivo.estadoEspecial.CompartirDa�o = true;
                break;

            case TipoEfecto.Miedo:
                if (Random.value < 0.3f)
                    objetivo.estadoEspecial.Paralizado = true;
                break;

            case TipoEfecto.Revivir:
                if (!yaActivado && objetivo.vida <= 0)
                {
                    objetivo.Revivir(10);
                    yaActivado = true;
                }
                break;
        }

        duracionTurnos--;
    }

    public bool Expirado => duracionTurnos <= 0;
}
