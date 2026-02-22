using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Anim_Controller : MonoBehaviour, IInfo
{
    // Componentes
    Animator animator;
    private Moviment_teclat movimentTeclat;
    private Moviment_vertical movimentVertical;
    private Dispar dispar;
        
    // Animaciones
    private int hashRun;
    private int hashJump;
    private int hashShotgun;
    private int hashAim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movimentTeclat = GetComponent<Moviment_teclat>();
        movimentVertical = GetComponent<Moviment_vertical>();
        dispar = GetComponent<Dispar>();
        

        // Guardo el nombre de las variables del animator en hashes para que unity las gestione más rápido
        hashRun = Animator.StringToHash("run");
        hashJump = Animator.StringToHash("jump");
        hashShotgun = Animator.StringToHash("shotgun");
        hashAim = Animator.StringToHash("aim");
        
        // animacion inicial
        animator.Play("Warrior Idle");
    }

    private RaycastHit hit;

    public void Info_Impacte_Superficie(RaycastHit hitInfo)
    {
        hit = hitInfo;
    }

    private void Update()
    {
        if (hit.collider is not null)
        {
            animator.SetFloat(hashRun, movimentTeclat.VelocidadReal);
            animator.SetBool(hashJump, false);
            if (movimentVertical.IsJumping)
            {
                animator.SetBool(hashJump, true);
            }
        }
        else
        {
            animator.SetBool(hashJump, false);
        }

        if (dispar.T_arma is not null)
        {
            animator.SetBool(hashShotgun, true);
        }
        else
        {
            animator.SetBool(hashShotgun, false);
        }

        if (dispar.T_arma is not null && dispar.EstaApuntando)
        {
            animator.SetBool(hashAim, true);
            
        }
        else
        {
            animator.SetBool(hashAim, false);
        }
    }

    private void Play_Rifle_Death()
    {
        animator.Play("Rifle Death");
    }

    private void OnEnable()
    {
        Player_Health.OnDie += Play_Rifle_Death;
    }
    
    private void OnDisable()
    {
        Player_Health.OnDie -= Play_Rifle_Death;
    }
}
