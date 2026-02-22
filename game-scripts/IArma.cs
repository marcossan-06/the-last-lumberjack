using UnityEngine;

public interface IArma 
{
    bool Disparar();
    bool Apuntar();
    bool CancelarApuntar();
    bool Carraga(int i_numero_projectils);
    void Agarrar(Transform t_ma);
    void Soltar();
}
