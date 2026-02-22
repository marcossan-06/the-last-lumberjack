using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameState_Manager : MonoBehaviour
{

    [SerializeField] private GameObject infoCanvas;
    [SerializeField] private GameObject controlsCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject paco;
    [SerializeField] private GameObject gamepadHelp;
    [SerializeField] private MonoBehaviour[] scriptsJuego;
    [SerializeField] private SoundController soundController;
    [SerializeField] private PlayerStats_UI playerStats;
    private Boolean _partidaIniciada;

    private void Start()
    {
        infoCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        
        menuCanvas.SetActive(true);
        
        paco.GetComponentInChildren<Animator>().enabled = false;
        paco.GetComponent<Moviment_Cub>().enabled = false;
        foreach (MonoBehaviour script in scriptsJuego)
        {
            script.enabled = false;
        }
    }

    private void Update()
    {
        // Mostrar la ayuda de mando
        if (Gamepad.current is not null && menuCanvas.activeInHierarchy && Touchscreen.current is null)
        {
            gamepadHelp.SetActive(true);
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                StartGame();
                soundController.ReproducirBattleMusic();
                Debug.Log("X pulsado - JUGAR");
            }

            if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                playerStats.IntentarMejorarEscopeta();
                Debug.Log("Triángulo pulsado - MEJORAR");
            }
        }
        else
        {
            gamepadHelp.SetActive(false);
        }
        
        if (Gamepad.current is not null && gameOverCanvas.activeInHierarchy && Touchscreen.current is null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                VolverMenu();
                soundController.ReproducirBattleMusic();
                Debug.Log("X pulsado - VOLVER AL MENÚ");
            }
        }
    }
    
    public void StartGame()
    {
        // Deshabilitar el cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        menuCanvas.SetActive(false);
        
        infoCanvas.SetActive(true);
        controlsCanvas.SetActive(true);
        
        paco.GetComponentInChildren<Animator>().enabled = true;
        paco.GetComponent<Moviment_Cub>().enabled = true;
        foreach (MonoBehaviour script in scriptsJuego)
        {
            script.enabled = true;
        }
        Time.timeScale = 1f;
    }

    
    // Todo esto ocurre cuando a paco le queda 0 de vida
    private void GameOver()
    {
        // Habilitar el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menuCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        infoCanvas.SetActive(false);
        paco.GetComponent<AudioSource>().enabled = false;
        
        foreach (MonoBehaviour script in scriptsJuego)
        {
            script.enabled = false;
        }
        
        Time.timeScale = 0.8f;
    }

    // y muestro el canvas de game over cuando acaba su animación de morir
    public void MostrarGameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0.2f;
    }
    
    public void VolverMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    private void OnEnable()
    {
        Player_Health.OnDie += GameOver;
        Eventos_Animaciones.OnDeathAnimationEnd += MostrarGameOver;
    }

    private void OnDisable()
    {
        Player_Health.OnDie -= GameOver;
        Eventos_Animaciones.OnDeathAnimationEnd -= MostrarGameOver;
    }
}
