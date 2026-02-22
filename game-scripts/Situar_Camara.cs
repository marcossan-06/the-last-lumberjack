using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Situar_Camara : MonoBehaviour
{
    Moviment_Camara moviment_camara;
    public List<Transform> lst_t_posicions_cam = new List<Transform>();
    int i_posicions = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moviment_camara = Camera.main.GetComponent<Moviment_Camara>();
        moviment_camara.t_personatge_a_seguir = transform;
        moviment_camara.v3_l_posicio_al_personatge = new Vector3(0f, 3f, -10f);

    }

    // Update is called once per frame
    void Update()
    {        
        if(lst_t_posicions_cam.Count == 0)
            return;

        if(Keyboard.current.f1Key.wasPressedThisFrame )
        {
            ++i_posicions;
            if (i_posicions == lst_t_posicions_cam.Count) i_posicions = 0;
            moviment_camara.v3_l_posicio_al_personatge = 
                transform.InverseTransformPoint(lst_t_posicions_cam[i_posicions].position);
        }
    }
}
