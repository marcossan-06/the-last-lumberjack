using UnityEngine;

public class AngryLogBoss : Enemigo
{

    [SerializeField] private GameObject efectoEspecial;
    public override void ProcesarImpacto(RaycastHit hit, float damage)
    {
        // El enemigo recibe daño A SU MANERA
        // (en este caso el hacha del enemigo puede BLOQUEAR perdigones)
        if (hit.collider == gameObject.GetComponent<Collider>())
        {
            QuitarVida(damage);
        }
    }
    
    protected override void EfectoEspecial()
    {
        Vector3 posicionSuelo = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        GameObject efecto = Instantiate(efectoEspecial, posicionSuelo, transform.rotation);
        Destroy(efecto, 20);
    }
}