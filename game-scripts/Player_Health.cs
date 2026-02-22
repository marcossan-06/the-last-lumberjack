using System;
using UnityEngine;

public class Player_Health : MonoBehaviour
{
    [SerializeField] private float maxHeath;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    [SerializeField] private Player_Healthbar healthBar;
    [SerializeField] private GameObject efectoMuerte;
    
    public static event Action OnReduceHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Debug.Log($"{gameObject.name} tiene {maxHeath} de vida");
        currentHealth = maxHeath;
        healthBar.UpdateHealthbar(currentHealth, maxHeath);
    }
    
    public void QuitarVida(float cantidad)
    {
        if (currentHealth > 0)
        {
            currentHealth -= cantidad;
            healthBar.UpdateHealthbar(currentHealth, maxHeath);
            Debug.Log($"{gameObject.name} ha recibido {cantidad} de daño. Vida restante: {currentHealth}");
            OnReduceHealth?.Invoke();
        }

        if (currentHealth <= 0 && currentHealth < maxHeath)
        {
            Morir();
        }
    }

    public void RecibirVida(float cantidad)
    {
        if (currentHealth > 0)
        {
            // No me paso del máximo de vida
            currentHealth = Math.Min(currentHealth + cantidad, maxHeath);
            healthBar.UpdateHealthbar(currentHealth, maxHeath);
            Debug.Log($"{gameObject.name} se ha curado {cantidad} de vida. Vida restante: {currentHealth}");
        }
    }

    [SerializeField] private GameObject paco;
    
    public static event Action OnDie;
    
    private void Morir()
    {
        Debug.Log($"{gameObject.name} ha muerto");
        paco.GetComponent<Moviment_Cub>().enabled = false;
        paco.GetComponent<Dispar>().enabled = false;
        
        OnDie?.Invoke();
    }
}
