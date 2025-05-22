using UnityEngine;

public enum TipoEfecto
{
    Asqueado,        // No se puede curar
    ReducirDa�o,     // Baja el da�o infligido
    Miedo,           // Posibilidad de perder turno
    Furia,           // Aumenta da�o
    CurarPorTurno,   // Cura al comienzo de cada turno
    Revivir,         // Revive una vez si muere
    RobarSalud,      // Drena HP a enemigos
    CompartirDa�o,   // Comparte da�o con aliados
    Sangrado         // Pierde vida por turno
}

