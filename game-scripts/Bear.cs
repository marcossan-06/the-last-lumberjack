using System;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Enemigo
{
    private List<RaycastHit> perdigonesImpactados;
    [SerializeField] private CapsuleCollider[] bodyColliders;
    [SerializeField] private CapsuleCollider headCollider;

    private void Start()
    {
        bodyColliders = GetComponentsInChildren<CapsuleCollider>();
    }
    
    public override void ProcesarImpacto(RaycastHit hit, float damage)
    {
        // El enemigo recibe daño A SU MANERA
        // (en este caso el oso tiene un collider para el headshot y más colliders)
        foreach (var bodyCollider in bodyColliders)
        {
            if (hit.collider == bodyCollider)
            {
                QuitarVida(damage);
            }
        }
                
        if (hit.collider == headCollider)
        {
            QuitarVida(damage * 2);
        }
    }
}

