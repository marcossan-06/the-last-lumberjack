using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OleadasController : MonoBehaviour
{
    [Header("Aviso oleada")]
    [SerializeField] private Transform infoCanvas;
    [SerializeField] private GameObject texto_oleada_prefab;
    private TextMeshProUGUI texto_oleada;
    
    [Header("Enemigos")]
    [SerializeField] private GameObject angryLog;
    [SerializeField] private GameObject angryLogGold;
    [SerializeField] private GameObject angryLogBoss;
    [SerializeField] private GameObject angryLogBossGold;
    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject bearGold;
    [SerializeField] private GameObject angryBear;
    [SerializeField] private GameObject angryBearGold;
    
    [Header("Zonas de Spawn")]
    [SerializeField] private Transform[] angryLogSpawns;
    [SerializeField] private Transform[] bearSpawns;

    private int oleadaActual;
    private int enemigosVivos;
    private int nivelEscopeta;

    [SerializeField] private float tiempoEntreOleadas;
    private float timer;

    private void Start()
    {
        texto_oleada = texto_oleada_prefab.GetComponent<TextMeshProUGUI>();
        oleadaActual = 1;
        nivelEscopeta = PlayerPrefs.GetInt("Nivel", 1);
        if (nivelEscopeta >= 2)
        {
            oleadaActual = nivelEscopeta / 2;
        }
        enemigosVivos = 0;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemigosVivos <= 0)
        {
            timer += Time.deltaTime;
            if (timer > tiempoEntreOleadas)
            {
                NuevaOleada();
                timer = 0;
            }
        }
        
    }
    
    void ActualizarTexto()
    {
        texto_oleada.text = "Oleada " + oleadaActual;
    }

    public void NuevaOleada()
    {
        // aviso oleada
        Debug.Log("NUEVA OLEADA: " + oleadaActual);
        ActualizarTexto();
        GameObject textoInstanciado = Instantiate(texto_oleada_prefab, infoCanvas);
        Debug.Log(textoInstanciado + "se ha instanciado");
        Destroy(textoInstanciado, 5);
        
        // Escalado del presupuesto para la dificultad de la oleada
        int puntosDificultad = oleadaActual * 3 + Mathf.RoundToInt(Mathf.Pow(oleadaActual, 1.5f));

        while (puntosDificultad > 0)
        {
            int random = Random.Range(1, 11);
            int randomGold = Random.Range(1, 101); // 1% de probabilidad de dorado (que da más x10 de almas)
            
            if (oleadaActual >= 12 && random >= 9 && puntosDificultad >= 25)
            {
                if (randomGold >= 100)
                {
                    Spawnear(angryBearGold, bearSpawns);
                }
                else
                {
                    Spawnear(angryBear, bearSpawns);   
                }
                puntosDificultad -= 25;
            }
            else if (oleadaActual >= 8 && random >= 8 && puntosDificultad >= 15)
            {
                if (randomGold >= 100)
                {
                    Spawnear(angryLogBossGold, angryLogSpawns);
                }
                else
                {
                    Spawnear(angryLogBoss, angryLogSpawns);   
                }
                puntosDificultad -= 15;
            }
            else if (oleadaActual >= 4 && random >= 6 && puntosDificultad >= 5)
            {
                if (randomGold >= 100)
                {
                    Spawnear(bearGold, bearSpawns);
                }
                else
                {
                    Spawnear(bear, bearSpawns);
                }
                puntosDificultad -= 5;
            }
            else
            {
                if (randomGold >= 100)
                {
                        Spawnear(angryLogGold, angryLogSpawns);
                }
                else
                {
                    Spawnear(angryLog, angryLogSpawns);
                }
                puntosDificultad -= 2;
            }
        }
        oleadaActual++;
    }

    public void Spawnear(GameObject enemigo, Transform[] spawns)
    {
        int randomSpawn = Random.Range(0, spawns.Length);
        Instantiate(enemigo, spawns[randomSpawn].position, Quaternion.identity);
        enemigosVivos++;
        Debug.Log($"{enemigo.name} ha spawneado. {enemigosVivos} enemigos vivos");
    }
    
    // Gestiono los enemigos vivos por eventos
    void OnEnable() 
    {
        Enemigo.OnEnemigoMuerto += RestarEnemigo;
    }

    void OnDisable() 
    {
        Enemigo.OnEnemigoMuerto -= RestarEnemigo;
    }

    void RestarEnemigo(Enemigo enemigo)
    {
        enemigosVivos--;

        if (enemigosVivos < 0)
        {
            enemigosVivos = 0;
        }
    }
}
