// Clase que representa un efecto aplicado a un luchador y su comportamiento por turno
using UnityEngine;

[System.Serializable]
public class EfectoActivo
{
    public string nombre;                    // Nombre del efecto
    public TipoEfecto tipo;                  // Tipo de efecto aplicado
    public int duracionTurnos;               // Cuántos turnos dura el efecto (excepto algunos permanentes)
    public int modificador;                  // Valor adicional que influye en el efecto
    public Luchador lanzador;                // Quién aplicó el efecto
    public int bonusFuriaSanidad = 0;
    public bool aplicado = false;

    // Método que aplica el efecto al objetivo cada turno
    public void AplicarEfectoPorTurno(Luchador objetivo, Luchador lanzadorIgnorado = null)
    {
        // El sangrado es permanente, los demás reducen duración
        if (tipo != TipoEfecto.Sangrado)
            duracionTurnos--;

        switch (tipo)
        {
            case TipoEfecto.Sangrado:
                Debug.Log($"{objetivo.nombre} sufre sangrado permanente.");
                objetivo.estadoEspecial.Sangrado = true;
                break;

            case TipoEfecto.Furia:
                if (!aplicado)
                {
                    objetivo.bonusDaño += modificador;
                    objetivo.estadoEspecial.FuriaRecibidaExtra = modificador;
                    aplicado = true;
                    Debug.Log($"{objetivo.nombre} gana +{modificador} de daño por Furia.");
                }
                break;


            case TipoEfecto.Asqueado:
                Debug.Log($"{objetivo.nombre} vomita del asco.");
                objetivo.estadoEspecial.Asqueado = true;
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
                // No se aplica cada turno, se aplica al ser creado.
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

    // Clase interna para efecto especial de FuriaFocalizada
    public class FuriaFocalizada
    {
        public Luchador objetivo;
        public int bonusDaño;
        public int turnosRestantes;
    }

    // Indica si el efecto ha expirado (excepto sangrado)
    public bool Expirado => tipo != TipoEfecto.Sangrado && duracionTurnos <= 0;
}
