using UnityEngine;

public class Moviment_Explosio : MonoBehaviour,IVelocitat
{
    Vector3 v3_g_velocitat = Vector3.zero;  

    public void Reinicia()
    {
        v3_g_velocitat = Vector3.zero;
    }

    public Vector3 V3_g_velocitat()
    {
        v3_g_velocitat = Vector3.MoveTowards(v3_g_velocitat, Vector3.zero, 4.5f * Time.deltaTime);
        return v3_g_velocitat;
    }

    public void AddExplosionForce(float f_forza, Vector3 v3_position,float f_radi,  float f_modificador)
    {
        float f_distancia = Vector3.Distance(v3_position, transform.position);
        if (f_distancia < f_radi )
        {
            float f_forza_a_distancia = -(f_forza * f_distancia / f_radi) + f_forza;
            f_forza_a_distancia /= 25f;
            f_modificador /= 5f;
            Vector3 v3_direccio_explosio = (transform.position - v3_position).normalized;

            v3_g_velocitat = v3_direccio_explosio * f_forza_a_distancia;

            v3_g_velocitat += Vector3.up * f_modificador;
        }
    }

}
