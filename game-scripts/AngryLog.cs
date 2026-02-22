using UnityEngine;

public class AngryLog : Enemigo
{
    public override void ProcesarImpacto(RaycastHit hit, float damage)
    {
        // El enemigo recibe daño A SU MANERA
        // (en este caso el hacha del enemigo puede BLOQUEAR perdigones)
        if (hit.collider == gameObject.GetComponent<Collider>())
        {
            QuitarVida(damage);
        }
    }
}
