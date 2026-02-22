using UnityEngine;
using UnityEngine.AI;

public class Bear_AnimController : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _agent;
    
    // Hashes mejor que strings
    private int hashSpeed;

    private int hashAttack;

    private int hashAttackType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        
        //Parámetros
        hashSpeed = Animator.StringToHash("Speed");
        hashAttack = Animator.StringToHash("Attack");
        hashAttackType = Animator.StringToHash("AttackType");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (Time.time >= tiempoSiguienteAtaque)
            {
                AnimarAtaque();
            }
        }
        float speed = _agent.velocity.magnitude;
        _animator.SetFloat(hashSpeed, speed);
    }

    private float tiempoSiguienteAtaque;
    [SerializeField] private float cooldownAtaque; // 1.5s
    private void AnimarAtaque()
    {
        int ataqueRandom = Random.Range(1,5);
        _animator.SetInteger(hashAttackType, ataqueRandom);
        _animator.SetTrigger(hashAttack);
        tiempoSiguienteAtaque = Time.time + cooldownAtaque;
    }
}
