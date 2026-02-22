using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento_Vertical : MonoBehaviour
{
    [SerializeField] private float _angulo;

    public float angulo
    {
        get => _angulo;
        set => _angulo = (360 + value % 360) % 360;
    }
    
    [SerializeField] private float radio = 0;
    [SerializeField] private Transform objetivo;
    [SerializeField] private Vector3 posExtra;
    [SerializeField] private Vector3 posExtraApuntar;
    private float f_sensibilidad;
    private Dispar dispar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dispar = GetComponentInParent<Dispar>();
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
        
        float rad = angulo * Mathf.Deg2Rad;
        transform.localPosition = posExtra + new Vector3(0, Mathf.Sin(rad), Mathf.Cos(rad)) * radio;
        transform.LookAt(objetivo.position+objetivo.TransformVector(posExtra));

        if (Touchscreen.current is null)
        {
            Debug.Log("NO HAY PANTALLA TÁCTIL");
            angulo = Mathf.Clamp(angulo + axis.y * f_sensibilidad, 150, 230);
        }
        else
        {
            // SI HAY PANTALLA TÁCTIL
            Debug.Log("HAY PANTALLA TÁCTILL");
            foreach (var touch in Touchscreen.current.touches)
            {
                if (touch.isInProgress && touch.startPosition.x.value > Screen.width * 0.4f)
                {
                    // Normalizo por altura de la pantalla para que la sensibilidad no varíe según esta
                    float deltaNormalizado = touch.delta.y.value / Screen.height;
                    // Aplicar sensibilidad (ajusto multiplicador al gusto)
                    float movimiento = deltaNormalizado * f_sensibilidad * 150f;
                    angulo = Mathf.Clamp(angulo + movimiento, 150, 230);
                    // Pongo el break para que no intenten mover la cámara más de un dedo a la vez
                    break;
                }
            }
        }
    }
    
    private Vector2 axis;
    public void OnLook(InputAction.CallbackContext context)
    {
        axis = context.ReadValue<Vector2>();
        Debug.Log($"Moviendo camara vertical: {axis}");
    }
}
