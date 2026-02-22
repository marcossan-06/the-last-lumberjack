using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{ 
    private Transform player;
    private NavMeshAgent agent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player is not null)
        {
            agent.SetDestination(player.position);
        }
    }
}
