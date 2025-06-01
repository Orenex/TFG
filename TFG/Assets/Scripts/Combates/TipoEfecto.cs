using UnityEngine;

// Enum que representa los distintos tipos de efectos que una carta puede aplicar a un luchador
public enum TipoEfecto
{
    Sangrado,             // Causa da�o cada turno de forma indefinida
    Furia,                // Aumenta el da�o infligido
    Asqueado,             // Causa mucho da�o cada turno
    CompartirDa�o,        // Refleja da�o recibido a otro objetivo
    FuriaFocalizada,      // Aumenta el da�o a un objetivo espec�fico
    Paralizado,           // Impide actuar en su turno
    Critico,              // Probabilidad de hacer el doble de da�o
    CondicionPerfecta,    // Las acciones con probablilidad siempre son perfectas
    ResucitarUnaVez,      // Revive una vez autom�ticamente al morir
    Confusion,            // Puede atacar a aliados si est� activo
    FuriaSanidad,         // Aumenta da�o en funci�n de sanidad perdida
    DanioEnArea,          // Afecta a m�ltiples objetivos a la vez


}
