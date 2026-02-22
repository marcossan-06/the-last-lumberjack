using UnityEngine;

public class Reiniciar : MonoBehaviour
{
    [SerializeField] float f_limit_vida_vertical = -30f;
    Vector3 v3_posicio_inicia = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        v3_posicio_inicia = transform.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if(transform.position.y < f_limit_vida_vertical)
        {
            transform.position = v3_posicio_inicia;
            foreach (IVelocitat ivelocitat in GetComponents<IVelocitat>())
            {
                MonoBehaviour monob = ivelocitat as MonoBehaviour;
                if (monob.enabled)
                    ivelocitat.Reinicia();
            }
        }            
    }
}
