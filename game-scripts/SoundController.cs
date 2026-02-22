using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    [Header("Sonidos de Pasos")]
    [SerializeField] private AudioSource audioPaco;
    [SerializeField] private AudioClip pasosHierba;
    [SerializeField] private AudioClip pasosMadera;
    [SerializeField] private AudioClip pasosAgua;
    [SerializeField] private float pitchSlowRun = 1.07f;
    [SerializeField] private float pitchSprint = 1.8f;
    
    [Header("Sonidos de Voz")]
    [SerializeField] private AudioSource audioVozPaco;
    [SerializeField] private AudioClip bostezo;
    [SerializeField] private AudioClip[] sonidosDanyo;

    [Header("Sonidos de UI")]
    [SerializeField] AudioSource audioUpgradeButton;
    [SerializeField] private AudioClip mejoraSuccess;
    [SerializeField] private AudioClip mejoraFail;
    
    [Header("Menu Music")]
    [SerializeField] private AudioSource audioMenuMusic;
    [SerializeField] private AudioClip[] epicMenuMusic;
    
    [Header("Battle Music")]
    private AudioSource audioBattleMusic;
    [SerializeField] private AudioClip[] battleMusic;


    private void Start()
    {
        audioBattleMusic = GetComponent<AudioSource>();
        ReproducirMenuMusic();
    }

    private void OnEnable()
    {
        Moviment_teclat.OnEstadoPasosCambiado += ManejarSonidoPasos;
        Player_Health.OnReduceHealth += ReproducirSonidosDanyo;
        Eventos_Animaciones.OnBostezoSound += ReproducirBostezo;
        PlayerStats_UI.OnShotgunUpgraded += ReproducirMejora;
        PlayerStats_UI.OnShotgunUpgradedFail += ReproducirMejoraFail;
    }

    private void OnDisable()
    {
        Moviment_teclat.OnEstadoPasosCambiado -= ManejarSonidoPasos;
        Player_Health.OnReduceHealth -= ReproducirSonidosDanyo;
        Eventos_Animaciones.OnBostezoSound -= ReproducirBostezo;
        PlayerStats_UI.OnShotgunUpgraded -= ReproducirMejora;
        PlayerStats_UI.OnShotgunUpgradedFail -= ReproducirMejoraFail;
    }
    
    private void ManejarSonidoPasos(int estadoMovimiento)
    {
        switch (estadoMovimiento)
        {
            case 0: // quieto
                if (audioPaco.isPlaying)
                {
                    audioPaco.Stop();
                }
                break;
            case 1: // slow run en hierba
                audioPaco.volume = 1.0f;
                audioPaco.pitch = pitchSlowRun;
                ActualizarClipPasos(pasosHierba);
                break;
            case 2: // sprint en hierba
                audioPaco.volume = 1.0f;
                audioPaco.pitch = pitchSprint;
                ActualizarClipPasos(pasosHierba);
                break;
            case 3: // slow run en madera
                audioPaco.volume = 0.7f;
                audioPaco.pitch = pitchSlowRun;
                ActualizarClipPasos(pasosMadera);
                break;
            case 4: // sprint en madera
                audioPaco.volume = 0.7f;
                audioPaco.pitch = pitchSprint;
                ActualizarClipPasos(pasosMadera);
                break;
            case 5: // slow run en agua
                audioPaco.volume = 1f;
                audioPaco.pitch = pitchSlowRun + 2f;
                ActualizarClipPasos(pasosAgua);
                break;
            case 6: // sprint en agua
                audioPaco.volume = 1f;
                audioPaco.pitch = pitchSprint + 2f;
                ActualizarClipPasos(pasosAgua);
                break;
        }
    }
    private void ActualizarClipPasos(AudioClip nuevoClip)
    {
        if (audioPaco.clip != nuevoClip)
        {
            audioPaco.clip = nuevoClip;
        }
        
        if (!audioPaco.isPlaying)
        {
            audioPaco.Play();
        }
    }

    public void ReproducirBostezo()
    {
        if (!audioPaco.isPlaying)
        {
            audioVozPaco.clip = bostezo;
            audioVozPaco.volume = 1.0f;
            audioVozPaco.pitch = 1f;
            audioVozPaco.loop = false;
            audioVozPaco.Play();
        }
    }

    public void ReproducirSonidosDanyo()
    {
        int randomClip = Random.Range(0, sonidosDanyo.Length);
        
        audioVozPaco.clip = sonidosDanyo[randomClip];
        if (!audioVozPaco.isPlaying)
        {
            audioVozPaco.Play();
        }
    }
    
    public void ReproducirMejora()
    {
        audioUpgradeButton.clip = mejoraSuccess;
        audioUpgradeButton.Play();
    }
    
    public void ReproducirMejoraFail()
    {
        audioUpgradeButton.clip = mejoraFail;
        audioUpgradeButton.Play();
    }
    
    public void ReproducirMenuMusic()
    {
        int randomClip = Random.Range(0, epicMenuMusic.Length);
        
        audioMenuMusic.clip = epicMenuMusic[randomClip];
        if (!audioMenuMusic.isPlaying)
        {
            audioMenuMusic.Play();
        }
    }
    
    // la uso en el onclick del botón de jugar
    public void ReproducirBattleMusic()
    {
        int randomClip = Random.Range(0, battleMusic.Length);
        
        audioBattleMusic.clip = battleMusic[randomClip];
        if (!audioBattleMusic.isPlaying)
        {
            audioBattleMusic.Play();
        }
    }
}
