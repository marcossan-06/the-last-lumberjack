using System;
using UnityEngine;
using TMPro;

public class PlayerStats_UI : MonoBehaviour
{
    private int almasTotales;
    private int costeMejora;
    private int nivelEscopeta;
    private float cadencia;
    public float Cadencia => cadencia;
    private float damagePerdigon;
    public float DamagePerdigon => damagePerdigon;
    
    [SerializeField] private ShotgunController shotgunController;
    
    [Header("UI Menu")]
    [SerializeField] private TextMeshProUGUI textoAlmasMenu;
    [SerializeField] private TextMeshProUGUI textoCoste;
    [SerializeField] private TextMeshProUGUI textoNivel;
    [SerializeField] private TextMeshProUGUI textoCadencia;
    [SerializeField] private TextMeshProUGUI textoDamage;
    
    [Header("UI En Partida")]
    [SerializeField] private TextMeshProUGUI textoAlmasPartida;

    void Start()
    {
        almasTotales = PlayerPrefs.GetInt("Almas", 0);
        costeMejora = PlayerPrefs.GetInt("Coste", 150);
        nivelEscopeta = PlayerPrefs.GetInt("Nivel", 1);
        cadencia = PlayerPrefs.GetFloat("Cadencia", 2f);
        damagePerdigon = PlayerPrefs.GetFloat("DamagePerdigon", 12f);
        ActualizarUI();
    }

    public void GuardarProgreso()
    {
        PlayerPrefs.SetInt("Almas", almasTotales);
        PlayerPrefs.SetInt("Coste", costeMejora);
        PlayerPrefs.SetInt("Nivel", nivelEscopeta);
        PlayerPrefs.SetFloat("Cadencia", cadencia);
        PlayerPrefs.SetFloat("DamagePerdigon", damagePerdigon);
    }

    public void AddAlmas(Enemigo enemigo)
    {
        if (enemigo is AngryLog)
        {
            if (enemigo.gameObject.name.StartsWith("AngryLog Gold")) // dorado x10
            {
                almasTotales += 100;
                Debug.Log("DETECTADO TEXTO GOLD + 100 almas");
            }
            else
            {
                almasTotales += 10; // normal
                Debug.Log("SE HAN SUMADO 10 ALMAS DE ANGRYLOG");
            }
        }
        else if (enemigo is Bear)
        {
            // si es negro 75, si no 25
            if (enemigo.gameObject.name.StartsWith("Angry Bear Gold")) // dorado x10
            {
                almasTotales += 750;
            }
            else if (enemigo.gameObject.name.StartsWith("Bear Gold")) // dorado x10
            {
                almasTotales += 250;
            } else if (enemigo.gameObject.name.StartsWith("Angry Bear")) // negro
            {
                almasTotales += 75;
            }
            else
            {
                almasTotales += 25; // normal
            }
           
        } 
        else if (enemigo is AngryLogBoss)
        {
            if (enemigo.gameObject.name.StartsWith("AngryLog Boss Gold")) // dorado x10
            {
                almasTotales += 500;
            }
            else
            {
                almasTotales += 50; // normal
            }
        }
        ActualizarUI();
    }

    public void ActualizarUI()
    {
        if (textoAlmasMenu is not null)
        {
            textoAlmasMenu.text = almasTotales.ToString();
            Debug.Log("TEXTO ALMAS ACTUALIZADO: " + textoAlmasMenu.text);
        }

        if (textoAlmasPartida is not null)
        {
            textoAlmasPartida.text = almasTotales.ToString();
            Debug.Log("TEXTO ALMAS EN PARTIDA: " + textoAlmasPartida.text);
        }
        
        if (textoCoste is not null)
        {
            textoCoste.text = costeMejora.ToString();
            Debug.Log("TEXTO COSTE ACTUALIZADO: " + textoCoste.text);
        }

        if (textoNivel is not null)
        {
            textoNivel.text = nivelEscopeta.ToString();
        }

        if (textoCadencia is not null)
        {
            textoCadencia.text = cadencia.ToString("F1");
        }

        if (textoDamage is not null)
        {
            textoDamage.text = (Mathf.RoundToInt(damagePerdigon * shotgunController.NumPerdigones)).ToString();
        }
        GuardarProgreso();
    }

    public static event Action OnShotgunUpgraded;
    public static event Action OnShotgunUpgradedFail;
    public void IntentarMejorarEscopeta()
    {
        if (almasTotales >= costeMejora)
        {
            almasTotales -= costeMejora;
            
            nivelEscopeta++;
            if (cadencia > 0)
            {
                cadencia -= 0.1f;
            }
            damagePerdigon *= 1.05f;
            OnShotgunUpgraded?.Invoke(); // el script dispar lo usa para establecer la nueva cadencia
            costeMejora = Mathf.RoundToInt(costeMejora * 1.25f);
            
            ActualizarUI();
            Debug.Log("ESCOPETA MEJORADA AL NIVEL " + nivelEscopeta);
        }
        else
        {
            Debug.Log("ALMAS INSUFICIENTES: " + almasTotales + " < " + costeMejora);
            OnShotgunUpgradedFail?.Invoke();
        }
    }

    private void OnEnable()
    {
        Enemigo.OnEnemigoMuerto += AddAlmas;
    }
    
    private void OnDisable()
    {
        Enemigo.OnEnemigoMuerto -= AddAlmas;
    }
}
