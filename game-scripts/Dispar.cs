using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dispar : MonoBehaviour
{
    [SerializeField] Transform T_ma;
    
    // Arma actual
    private Transform t_arma;
    public Transform T_arma{get => t_arma;}
    
    private IArma i_arma;
    public IArma I_arma{get => i_arma;}

    // Detectar arma cercana
    private IArma i_armaDetectada; // Script del arma detectada
    public IArma I_armaDetectada{get => i_armaDetectada;}
    
    private Transform t_armaDetectada; // El arma detectada
    public Transform  T_armaDetectada{get => t_armaDetectada;}
    
    private bool estaApuntando;

    public bool EstaApuntando
    {
        get { return estaApuntando; }
    }

    // Input
    private bool b_shoot_input;
    private bool b_aim_input;
    public bool B_aim_input{get => b_aim_input;}
    private bool  b_pickUp_input;

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("OnPickUp llamado");
            b_pickUp_input = true;
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (t_arma is not null)
        {
            if (context.control.device is Mouse)
            {   
                // Hay que mantener pulsado
                if (context.performed)
                {
                    Debug.Log("OnAim llamado");
                    b_aim_input = true;
                }
                if (context.canceled)
                {
                    Debug.Log("OnAim terminado");
                    b_aim_input = false;
                }
            }
            
            if (context.control.device is Gamepad)
            {
                if (Touchscreen.current == null)
                {
                    // Hay que mantener pulsado (Mando)
                    if (context.performed)
                    {
                        Debug.Log("OnAim llamado");
                        b_aim_input = true;
                    }
                    if (context.canceled)
                    {
                        Debug.Log("OnAim terminado");
                        b_aim_input = false;
                    }
                }
                else
                {   
                    // Activa y desactiva (Pantalla táctil)
                    if (context.performed)
                    {
                        b_aim_input = !b_aim_input;
                    }
                }
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("OnAttack llamado");
            b_shoot_input = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Apuntar
        if (b_aim_input && t_arma is not null)
        {
            estaApuntando = true;
            i_arma.Apuntar();
        }
        else
        {
            estaApuntando = false;
            if (t_arma is not null)
            {
                i_arma.CancelarApuntar();   
            }
        }
        

        // Disparar
        if (b_shoot_input && t_arma is not null && estaApuntando)
        {
            Debug.Log("Disparo");
            i_arma.Disparar();
        }

        if (b_pickUp_input)
        {
            // Soltar arma si tiene
            if (t_arma is not null)
            {
                Debug.Log("Soltar (E)");
                i_arma.Soltar();

                // Limpio todo
                i_armaDetectada = null;
                t_armaDetectada = null;
                t_arma = null;
                i_arma = null;
            }
            // Coger arma si no tiene y puede
            else if (i_armaDetectada is not null)
            {
                Debug.Log("Recoger (E)");
                t_arma = t_armaDetectada;
                i_arma = i_armaDetectada;
                i_arma.Agarrar(T_ma);

                i_armaDetectada = null;
                t_armaDetectada = null;
            }
        }
        b_shoot_input = false;
        b_pickUp_input = false;
    }

    // Paco entra en collider con trigger activado
    private void OnTriggerEnter(Collider other)
    {
        // Si paco ya tiene una escopeta detectada no puede detectar otra
        if (i_armaDetectada == null)
        {
            // Comprobar que es un arma
            IArma i_arma = other.gameObject.GetComponentInParent<IArma>();

            if (i_arma is not null && t_arma is null)
            {
                t_armaDetectada = other.transform;
                i_armaDetectada = i_arma;
                if (i_arma is ShotgunController shotgunController)
                {
                    shotgunController.MostrarTexto();
                }
                Debug.Log("Arma detectada. Pulsa 'E' para recoger.");
            }
        }
    }


    // Paco sale del collider
    private void OnTriggerExit(Collider other)
    {
        IArma i_arma = other.gameObject.GetComponentInParent<IArma>();
        
        if (i_arma is not null && i_arma == i_armaDetectada)
        {
            if (i_arma is ShotgunController shotgunController)
            {
                shotgunController.OcultarTexto();
            }
            Debug.Log("Arma fuera de alcance.");
            i_armaDetectada = null;
            t_armaDetectada = null;
        } 
    }
}
