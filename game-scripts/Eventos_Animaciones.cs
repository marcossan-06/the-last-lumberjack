using System;

using UnityEngine;

public class Eventos_Animaciones : MonoBehaviour
{
    // Este script es para propagar los eventos de animación de paco a otros scripts para que
    // ejecuten los métodos que dependen de las animaciones de paco
    public static event Action OnDeathAnimationEnd;
    public static event Action OnBostezoSound;

    public void DeathAnimationEnd()
    {
        OnDeathAnimationEnd?.Invoke();
    }
    
    public void BostezoSound()
    {
        OnBostezoSound?.Invoke();
    }
}
