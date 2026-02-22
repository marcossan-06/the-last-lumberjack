using UnityEngine;

public class Moviment_Camara : MonoBehaviour
{
    public Transform t_personatge_a_seguir;
    public Vector3 v3_l_posicio_al_personatge = Vector3.zero;   
    Vector3 v3_currentVelocity = Vector3.zero;
    public float f_smoothTime = 0.25f;
    public bool b_orientar = true;


    // Update is called once per frame
    void Update()
    {
        if(t_personatge_a_seguir is null)
            return;

        //posicio
        transform.position = Vector3.SmoothDamp(transform.position,
            t_personatge_a_seguir.TransformPoint(v3_l_posicio_al_personatge),
            ref v3_currentVelocity, f_smoothTime);

        // //oritacio
        // if(b_orientar )
        //     transform.rotation = Quaternion.Slerp(transform.rotation,
        //         t_personatge_a_seguir.rotation, Time.deltaTime / f_smoothTime);

    }
}
