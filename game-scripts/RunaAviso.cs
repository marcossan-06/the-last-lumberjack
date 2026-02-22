using System.Collections.Generic;
using UnityEngine;

public class RunaAviso : MonoBehaviour
{
    [SerializeField] private Material materialAzul;
    [SerializeField] private Material materialRojo;

    private MeshRenderer meshRenderer;
    
    // Lista de enemigos que están dentro del trigger
    private HashSet<GameObject> enemigosDentro;

    void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        enemigosDentro = new HashSet<GameObject>();
        meshRenderer.material = materialAzul;
    }

    private void EliminarEnemigo(Enemigo enemigo)
    {
        // Solo resto si estaba dentro de esta runa
        if (enemigo is not null && enemigosDentro.Contains(enemigo.gameObject))
        {
            enemigosDentro.Remove(enemigo.gameObject);
            Debug.Log("ENEMIGO ELIMINADO DE LA LISTA" + enemigo.name);
        }
    }

    void Update()
    {
        if (enemigosDentro.Count > 0)
            meshRenderer.material = materialRojo;
        else
            meshRenderer.material = materialAzul;
    }

    // Añado o quito de la lista de enemigos que están según van entrando y saliendo
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemigosDentro.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemigosDentro.Remove(other.gameObject);
        }
    }

    void OnEnable()
    {
        Enemigo.OnEnemigoMuerto += EliminarEnemigo;   
    }

    void OnDisable()
    {
        Enemigo.OnEnemigoMuerto -= EliminarEnemigo;   
    }
}