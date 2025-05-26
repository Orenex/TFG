using UnityEngine;

// Enum que representa los distintos tipos de efectos que una carta puede aplicar a un luchador.
public enum TipoEfecto
{
    Sangrado,             // Causa da�o cada turno de forma indefinida
    Furia,                // Aumenta el da�o infligido
    Asqueado,             // Causa mucho da�o cada turno
    CompartirDa�o,        // Refleja da�o recibido a otro objetivo
    FuriaFocalizada,      // Aumenta el da�o a un objetivo espec�fico
    Paralizado,           // Impide actuar
    Critico,              // Probabilidad de hacer el doble de da�o
    CondicionPerfecta,    // Puede usarse para efectos especiales positivos
    ResucitarUnaVez,      // Revive una vez al morir
    Confusion,            // Puede atacar a aliados si est� activo
    FuriaSanidad,         // Escala el da�o seg�n cu�nta sanidad se ha perdido
    DanioEnArea           // Afecta a m�ltiples objetivos a la vez
}
