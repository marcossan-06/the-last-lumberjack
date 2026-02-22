using UnityEngine;

public class Rotacio_Cub : MonoBehaviour
{

    public float f_angle = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, f_angle);
    }
}
