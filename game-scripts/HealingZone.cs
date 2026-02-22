using System;
using UnityEngine;

public class HealingZone : MonoBehaviour
{
    [SerializeField] private float saludPorSegundo;
    [SerializeField] private float duracion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     Destroy(gameObject, duracion);   
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Health player_health = other.GetComponent<Player_Health>();
            if (player_health != null)
            {
                player_health.RecibirVida(saludPorSegundo * Time.deltaTime);
            }
        }
    }
}
