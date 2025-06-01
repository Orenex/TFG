using UnityEngine;

// Enum que representa los distintos tipos de efectos que una carta puede aplicar a un luchador
public enum TipoEfecto
{
    Sangrado,             // Causa daño cada turno de forma indefinida
    Furia,                // Aumenta el daño infligido
    Asqueado,             // Causa mucho daño cada turno
    CompartirDaño,        // Refleja daño recibido a otro objetivo
    FuriaFocalizada,      // Aumenta el daño a un objetivo específico
    Paralizado,           // Impide actuar en su turno
    Critico,              // Probabilidad de hacer el doble de daño
    CondicionPerfecta,    // Las acciones con probablilidad siempre son perfectas
    ResucitarUnaVez,      // Revive una vez automáticamente al morir
    Confusion,            // Puede atacar a aliados si está activo
    FuriaSanidad,         // Aumenta daño en función de sanidad perdida
    DanioEnArea,          // Afecta a múltiples objetivos a la vez


}
