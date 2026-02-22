using UnityEngine;
using UnityEngine.InputSystem;

public class Moviment_vertical : MonoBehaviour, IVelocitat,IInfo
{
    CharacterController cc_goku;

    [SerializeField] float f_impuls_bot = 7f;
    public float F_impuls_bot { get { return f_impuls_bot; } set { f_impuls_bot = Mathf.Max(value, 0f); } }

    private Vector3 v3_l_velocitat_total = Vector3.zero;

    // Solución al reseteo de salto en las rampas
    // (Al saltar en una rampa el SphereCast sigue detectando la colision en los primeros momentos del salto
    // por lo que la velocidad se reseteaba y no se hacía el impulso del salto)
    // Entonces creo un pequeño tiempo al inicio del salto en el que no resetearé la velocidad
    private float jumpTime;
    private float graceTime = 0.1f;
    
    // Input
    private bool b_jump_input;
    private bool isJumping;
    public bool IsJumping{ get { return isJumping; } set { isJumping = value; } }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (raycastHit.collider is not null) // si está tocando el suelo
        {
            if (context.performed)
            {
                b_jump_input = true;
            }
        }
        Debug.Log(context.phase);
    }

    public Vector3 V3_g_velocitat()
    {
        //bot
        if (raycastHit.collider is not null) // si está tocando el suelo
        {
            // incremento el tiempo
            jumpTime += Time.deltaTime;
            isJumping = false;

            if (b_jump_input)
            {
                v3_l_velocitat_total = new Vector3(0f, f_impuls_bot, 0f);
                isJumping = true;
                b_jump_input = false;
                // reseteo tiempo porque es un nuevo salto
                jumpTime = 0f;
            }
            // Cuando pasa el tiempo reseteo la velocidad
            else if (jumpTime > graceTime)
            {
                v3_l_velocitat_total = Vector3.zero;
            }
        }



        if ((cc_goku.collisionFlags & CollisionFlags.Above) != 0)
        {
            v3_l_velocitat_total = Vector3.zero;
        }

        //gravetat
        v3_l_velocitat_total += Physics.gravity * Time.deltaTime;

        return transform.TransformVector( v3_l_velocitat_total );
    }
    public void Reinicia()
    {
        v3_l_velocitat_total = Vector3.zero;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cc_goku = GetComponent<CharacterController>();
    }

    RaycastHit raycastHit;
    public void Info_Impacte_Superficie(RaycastHit hitInfo)
    {
        raycastHit = hitInfo;
    }
}
