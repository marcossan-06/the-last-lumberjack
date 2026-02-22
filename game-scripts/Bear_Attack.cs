using UnityEngine;

public class Bear_Attack : MonoBehaviour
{
    [SerializeField] private float damage;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player_Health>().QuitarVida(damage);
        }
    }
}
