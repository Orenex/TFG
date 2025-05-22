[System.Serializable]
public class EstadoEspecial
{
    public bool Asqueado;
    public bool CompartirDaño;
    public bool Paralizado;

    public void Reiniciar()
    {
        Asqueado = false;
        CompartirDaño = false;
        Paralizado = false;
    }
}
