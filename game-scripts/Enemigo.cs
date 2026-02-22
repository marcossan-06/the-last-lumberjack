using System;
using UnityEngine;

public abstract class Enemigo : MonoBehaviour
{
    [SerializeField] protected float maxHeath;
    protected float currentHealth;
    [SerializeField] protected Enemy_HealthBar healthBar;
    [SerializeField] private GameObject efectoMuerte;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Debug.Log($"{gameObject.name} tiene {maxHeath} de vida");
        currentHealth = maxHeath;
        healthBar.UpdateHealthbar(currentHealth, maxHeath);
    }
    
    // abstract me obliga a definir los métodos en los hijos, virtual no
    // por eso todos los enemigos deben de poder procesae un impacto para quitarles vida
    // en cambio no todos los enemigos van a tener un efecto especial
    public abstract void ProcesarImpacto(RaycastHit hit, float damage);

    protected virtual void EfectoEspecial()
    {
        
    }
    
    public void QuitarVida(float cantidad)
    {
        if (currentHealth > 0)
        {
            currentHealth -= cantidad;   
            healthBar.UpdateHealthbar(currentHealth, maxHeath);
            Debug.Log($"{gameObject.name} ha recibido {cantidad} de daño. Vida restante: {currentHealth}");
        }

        if (currentHealth <= 0)
        {
            Morir();
        }
    }

    
    public static event Action<Enemigo> OnEnemigoMuerto;
    private bool _yaMuerto;
    private void Morir()
    {
        // Para no matarlo cuando ya está muerto y evitar fallos en el contador de enemigos vivos
        if (_yaMuerto) return;
        _yaMuerto = true;
        OnEnemigoMuerto?.Invoke(this);
        Debug.Log($"{gameObject.name} ha muerto");
        GameObject efecto = Instantiate(efectoMuerte, transform.position, transform.rotation);
        // hace su efecto especial de muerte (SI TIENE)
        EfectoEspecial();
        Destroy(efecto, 3);
        Destroy(gameObject);
    }
}