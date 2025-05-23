[System.Serializable]
public class EstadoEspecial
{
    public bool Asqueado;
    public bool CompartirDaño;
    public bool Paralizado;
    public int FuriaRecibidaExtra = 0;
    public bool Critico = false;
    public bool CondicionPerfecta = false;
    public bool ResucitarUnaVez = false;
    public bool Confusion = false;
    public Luchador ReflejarDanioA = null;



    public void Reiniciar()
    {
        Asqueado = false;
        CompartirDaño = false;
        Paralizado = false;
        FuriaRecibidaExtra = 0;
        Critico = false;
        CondicionPerfecta = false;
        Confusion = false;
        ReflejarDanioA = null;


    }
}
