using UnityEngine;

// Clase que almacena estados especiales activos sobre un luchador durante el combate
[System.Serializable]
public class EstadoEspecial
{
    public bool Asqueado;                  // Pierde vida por turno
    public bool CompartirDa�o;             // Comparte da�o recibido
    public bool Paralizado;                // Pierde el turno
    public int FuriaRecibidaExtra = 0;     // Recibe m�s da�o
    public bool Critico = false;           // Posibilidad de golpe cr�tico
    public bool CondicionPerfecta = false; // Estado positivo general
    public bool ResucitarUnaVez = false;   // Revive autom�ticamente una vez
    public bool Confusion = false;         // Puede atacar a aliados
    public Luchador ReflejarDanioA = null; // Enlace para reflejar da�o recibido
    public bool Sangrado;                  // Pierde vida por turno

    // Reinicia todos los estados al inicio de un nuevo turno
    public void Reiniciar()
    {
        Debug.Log("ReiniciandoLuchador");
        Asqueado = false;
        CompartirDa�o = false;
        Paralizado = false;
        FuriaRecibidaExtra = 0;
        Critico = false;
        CondicionPerfecta = false;
        Confusion = false;
        ReflejarDanioA = null;
        Sangrado = false;
    }
}
