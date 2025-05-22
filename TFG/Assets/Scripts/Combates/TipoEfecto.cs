using UnityEngine;

public enum TipoEfecto
{
    Asqueado,        // No se puede curar
    ReducirDaño,     // Baja el daño infligido
    Miedo,           // Posibilidad de perder turno
    Furia,           // Aumenta daño
    CurarPorTurno,   // Cura al comienzo de cada turno
    Revivir,         // Revive una vez si muere
    RobarSalud,      // Drena HP a enemigos
    CompartirDaño,   // Comparte daño con aliados
    Sangrado         // Pierde vida por turno
}

