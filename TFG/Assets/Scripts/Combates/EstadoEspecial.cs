[System.Serializable]
public class EstadoEspecial
{
    public bool Asqueado;
    public bool CompartirDa�o;
    public bool Paralizado;

    public void Reiniciar()
    {
        Asqueado = false;
        CompartirDa�o = false;
        Paralizado = false;
    }
}
