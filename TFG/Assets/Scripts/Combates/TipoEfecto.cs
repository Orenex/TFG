using UnityEngine;

// Enum que representa los distintos tipos de efectos que una carta puede aplicar a un luchador.
public enum TipoEfecto
{
    Sangrado,             // Causa daño cada turno de forma indefinida
    Furia,                // Aumenta el daño infligido
    Asqueado,             // Causa mucho daño cada turno
    CompartirDaño,        // Refleja daño recibido a otro objetivo
    FuriaFocalizada,      // Aumenta el daño a un objetivo específico
    Paralizado,           // Impide actuar
    Critico,              // Probabilidad de hacer el doble de daño
    CondicionPerfecta,    // Puede usarse para efectos especiales positivos
    ResucitarUnaVez,      // Revive una vez al morir
    Confusion,            // Puede atacar a aliados si está activo
    FuriaSanidad,         // Escala el daño según cuánta sanidad se ha perdido
    DanioEnArea           // Afecta a múltiples objetivos a la vez
}
