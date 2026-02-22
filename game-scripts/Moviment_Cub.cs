using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;


public class Moviment_Cub : MonoBehaviour
{ 

    private float f_sensibilidad;
    
    [SerializeField] float f_altura_extra = 0.1f;


    CharacterController cc_goku;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc_goku = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Touchscreen.current is null)
        {
            if (Gamepad.current is null)
            {
                // sens pc
                f_sensibilidad = 0.25f;
            }
            else
            {
                // sens mando
                f_sensibilidad = 2.5f;
            }

        }
        else
        {
            // sens pantalla tactil
            f_sensibilidad = 1f;
        }
        Debug.Log("sensibilidad" + f_sensibilidad);
        
        //aplicar velocitat total
        Vector3 v3_g_velocitat_total = Vector3.zero ;

        RaycastHit rchit;
        

        float sphereRadius = cc_goku.radius * 0.9f;
        Physics.SphereCast(transform.position, sphereRadius, Vector3.down, out rchit, (cc_goku.height / 2f) - sphereRadius + f_altura_extra);
        

        foreach (IInfo info in GetComponents<IInfo>())
        {
            MonoBehaviour monob = info as MonoBehaviour;
            if (monob.enabled)
                info.Info_Impacte_Superficie(rchit);
        }

        foreach (IVelocitat ivelocitat in GetComponents<IVelocitat>())
        {
            MonoBehaviour  monob =  ivelocitat as MonoBehaviour;
            if(monob.enabled ) 
                v3_g_velocitat_total += ivelocitat.V3_g_velocitat();
        }

        cc_goku.Move(v3_g_velocitat_total * Time.deltaTime);
        
        

        if (rchit.collider is not null)
        {
            if (Touchscreen.current is null)
            {
                transform.Rotate(Vector3.up, look_input.x * f_sensibilidad, Space.Self);
            }
            else
            {
                // SI HAY PANTALLA TÁCTIL
                Debug.Log("HAY PANTALLA TÁCTIL");
                foreach (var touch in Touchscreen.current.touches)
                {
                    if (touch.isInProgress && touch.startPosition.x.value > Screen.width * 0.4f)
                    {
                        // Normalizo el delta según el ancho de la pantalla (porque si no la senssibilidad varía según la resolución)
                        float normalizedDelta = touch.delta.x.value / Screen.width;
                        // Aplicar sensibilidad y rotación (en vez de modificar la sensibilidad por ahí, ajusto un poco el multiplicador)
                        float rotationAmount = normalizedDelta * f_sensibilidad * 400f;
                        transform.Rotate(Vector3.up, rotationAmount, Space.Self);
                        // Pongo el break para que no intenten mover la cámara más de un dedo a la vez
                        break;
                    }
                }
            }
        }
    }
    
    //Input
    private Vector2 look_input;
    

    public void OnLook(InputAction.CallbackContext context)
    {
        look_input = context.ReadValue<Vector2>();
    }
}
