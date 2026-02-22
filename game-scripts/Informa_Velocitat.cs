using UnityEngine;

public class Informa_Velocitat : MonoBehaviour,IVelocitat
{
    Vector3 v3_pos_anterior = Vector3.zero;
    Vector3 v3_g_velocitat = Vector3.zero;

    public Vector3 V3_g_velocitat()
    {
       return v3_g_velocitat;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        v3_pos_anterior = transform.position;
    }
        
    private void LateUpdate()
    {
        v3_g_velocitat = (transform.position - v3_pos_anterior) / Time.deltaTime;
        v3_pos_anterior = transform.position;
    }

    public void Reinicia()
    {
        v3_g_velocitat = Vector3.zero;
    }
}
