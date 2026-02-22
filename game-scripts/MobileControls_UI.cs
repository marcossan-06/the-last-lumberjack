using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MobileControls_UI : MonoBehaviour
{
    private Dispar dispar;
    [SerializeField] private GameObject botonRecogerEscopeta;
    [SerializeField] private GameObject botonApuntar;
    [SerializeField] private GameObject botonDisparar;
    [SerializeField] private GameObject panelBotones;
    [SerializeField] private GameObject panelMiraEscopeta;
    
    void Awake()
    {
        dispar = GetComponent<Dispar>();
        botonRecogerEscopeta.SetActive(false);
        botonApuntar.SetActive(false);
        botonDisparar.SetActive(false);
        panelMiraEscopeta.SetActive(false);
    }

    private void Update()
    {
        
        // Si no detecta pantalla táctil no muestra botones
        if (Touchscreen.current == null)
        {
            panelBotones.SetActive(false); 
        }
        // Controles movil apuntar y disparar
        if (dispar.T_arma is null)
        {
            botonApuntar.SetActive(false);
            botonDisparar.SetActive(false);
            
        }
        else
        {
            botonApuntar.SetActive(true);
            botonDisparar.SetActive(true);
        }
        
        if (dispar.B_aim_input && dispar.T_arma is not null)
        {
            panelMiraEscopeta.SetActive(true);
        }
        else
        {
            panelMiraEscopeta.SetActive(false);
        }
    }
    
    // Paco tiene un arma al alcance
    private void OnTriggerEnter(Collider other)
    {
        // Comprobar que es un arma
        IArma i_arma = other.gameObject.GetComponentInParent<IArma>();

        if (i_arma is not null && dispar.T_arma is null)
        {
            if (botonRecogerEscopeta is not null)
            {
                botonRecogerEscopeta.SetActive(true);
            }
        }
    }


    // Paco ya no está al alcance del arma
    private void OnTriggerExit(Collider other)
    {
        IArma i_arma = other.gameObject.GetComponentInParent<IArma>();
        if (i_arma is not null)
        {
            if (botonRecogerEscopeta is not null)
            {
                botonRecogerEscopeta.SetActive(false);
            }
        } 
    }
}