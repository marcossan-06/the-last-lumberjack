using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Moviment_teclat : MonoBehaviour,IVelocitat, IInfo
{
    CharacterController cc_goku;
    private bool jugadorMuerto;
    
    [Header("Velocidades")] // Para aclararse en el inspector
    [SerializeField] float f_velocitat_base;
    [SerializeField] float f_velocitat_sprint;
    private float f_velocitat;
    
    // Para acceder a la velocidad desde otros scripts (como el de animaciones)
    private float velocidadReal;
    public float VelocidadReal
    {
        get => velocidadReal;
        set => velocidadReal = value;
    }

    // Audio con eventos que lo manejo desde el SoundController
    public static event Action<int> OnEstadoPasosCambiado;
    private int estadoPasosActual = -1;

    private Dispar dispar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc_goku = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        dispar = GetComponent<Dispar>();
    } 
    
    // Input
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private bool b_sprintInput;
    
    // Evento de Unity del Input Sprint (es distintos entre mando y teclado)
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.control.device is Keyboard)
        {
            // Hay que mantener pulsado
            if (context.performed)
            {
                b_sprintInput = true;
            }
            if (context.canceled)
            {
                b_sprintInput = false;
            }
            return;
        }

        if (context.control.device is Gamepad)
        {
            // Activa y desactiva
            if (context.performed)
            {
                b_sprintInput = !b_sprintInput;
            }
        }
    }

    Vector3 v3_g_velocitat_total = Vector3.zero;

    public Vector3 V3_g_velocitat()
    {
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();

        //Condiciones para poder sprintar (solo puede sprintar si va hacia delante)
        if (Touchscreen.current != null && moveInput.y > 0.99f)
        {
            b_sprintInput = true;
        }
        bool b_isMovingForward = moveInput.y > 0.1f;
        bool b_isSprint = b_sprintInput && b_isMovingForward;
        


        // Si esta esprintando usamos la velocidad de sprint, si no, velocidad base
        f_velocitat = b_isSprint && !dispar.EstaApuntando ? f_velocitat_sprint : f_velocitat_base;
        
        // velocitat horitzontal
        if (raycastHit.collider is not null)
        {
            if (jugadorMuerto)
            {
                CambiarEstadoPasos(0); // No hay pasos
                return v3_g_velocitat_total;
            }
            Vector3 v3_l_velocitat_total = new Vector3(moveInput.x, 0f, moveInput.y);
            
            //sistema global
            v3_g_velocitat_total = Vector3.ProjectOnPlane( 
                transform.TransformVector(v3_l_velocitat_total), raycastHit.normal);

            // evitar velocidad residual
            bool seMueve = moveInput.magnitude >= 0.1f;
            if (!seMueve)
            {
                v3_g_velocitat_total = Vector3.zero;
                // Cuando el personaje se para desactivo el input del sprint
                b_sprintInput = false;
                    CambiarEstadoPasos(0); // Parado
            }
            
            v3_g_velocitat_total += v3_g_velocitat_total.normalized * f_velocitat;
            velocidadReal = v3_g_velocitat_total.magnitude;
            if (seMueve)
            {
                if (velocidadReal > 5f)
                {
                    if (raycastHit.collider.CompareTag("Wood"))
                    {
                        CambiarEstadoPasos(4); // Sprint en madera 
                    }
                    else if (raycastHit.collider.CompareTag("Water"))
                    {
                        CambiarEstadoPasos(6); // Sprint en agua
                    }
                    else
                    {
                        CambiarEstadoPasos(2); // Sprint
                    }
                }
                else
                {
                    if (raycastHit.collider.CompareTag("Wood"))
                    {
                        CambiarEstadoPasos(3); // Slow Run en madera
                    }
                    else if (raycastHit.collider.CompareTag("Water"))
                    {
                        CambiarEstadoPasos(5); // Slow Run en agua
                    }
                    else
                    {
                        CambiarEstadoPasos(1); // Slow Run
                    }
                }
            }
        }
        else
        {
            CambiarEstadoPasos(0); // Saltando (no hay pasos)
        }

        return  v3_g_velocitat_total;
    }

    private void CambiarEstadoPasos(int nuevoEstado)
    {
        if (nuevoEstado != estadoPasosActual)
        {
            estadoPasosActual = nuevoEstado;
            OnEstadoPasosCambiado?.Invoke(nuevoEstado);
        }
    }

    public void Reinicia()
    {
        v3_g_velocitat_total = Vector3.zero;
    }

    RaycastHit raycastHit;
    public void Info_Impacte_Superficie(RaycastHit hitInfo)
    {
        raycastHit = hitInfo;
    }

    private void SetMuerto()
    {
        jugadorMuerto = true;
    }

    private void OnEnable()
    {
        Player_Health.OnDie += SetMuerto;
    }
    
    private void OnDisable()
    {
        Player_Health.OnDie -= SetMuerto;
    }
}
