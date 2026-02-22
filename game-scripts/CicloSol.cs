using UnityEngine;

/// <summary>Componente que gira un GameObject para simular un ciclo de dia/noche</summary>
public class CicloSol : MonoBehaviour
{
    [SerializeField] private float duracionDiaSeg = 900;
    private float _inicio;
    protected void Start()
    {
        _inicio = Random.Range(0f, 360f);
    }

    protected void Update()
    {
        var rotacion = (_inicio + Time.time / duracionDiaSeg * 360f) % 360f;
        transform.rotation = Quaternion.Euler(rotacion, 0, 0);
    }
}