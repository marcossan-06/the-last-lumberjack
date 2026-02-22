using UnityEngine;
using UnityEngine.InputSystem;


public class ShotgunController : MonoBehaviour, IArma
{
    [SerializeField] private Transform T_boca;
    [SerializeField] private Transform T_efecto;
    private Rigidbody rb;
    [SerializeField] Collider coll_solido;
    [SerializeField] Transform T_referencia_mano;
    [SerializeField] private ParticleSystem auraShotgun;
    
    // Camera
    [SerializeField] private Transform camera;
    [SerializeField] private Moviment_Camara movimentCamara;
    [SerializeField] private Transform T_aimCamera;
    [SerializeField] private Transform T_normalCamera;
    [SerializeField] private float speedCamera;
    
    // Texto coger
    private Vector3 textOffset;
    private GameObject _textCanvas;
    
    private bool _isAiming;
    

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        textOffset = new Vector3(0, 1.2f, 0);
        _textCanvas = GameObject.Find("ShotgunText Canvas");
        OcultarTexto();
        Debug.Log("CANVAS START: " + _textCanvas);
        

        if (_playerStatsUI is null)
        {
            _playerStatsUI = FindFirstObjectByType<PlayerStats_UI>();
        }
        
        // CARGO LOS DATOS GUARDADOS DE LA ESCOPETA
        cadencia = PlayerPrefs.GetFloat("Cadencia", 2f);
        damagePorPerdigon = PlayerPrefs.GetFloat("DamagePerdigon", 12f);
    }

    public void Agarrar(Transform t_ma)
    {
        OcultarTexto();
        
        // Descativar físicas del RigidBody para que al cogerla no se caiga debajo del suelo
        if (rb is not null)
        {
            rb.isKinematic = true;
        }
        // Desactivar el Box Collider para que el personaje no se tropiece con la escopeta
        if (coll_solido is not null)
        {
            coll_solido.enabled = false;
        }

        // Desactivar aura de la escopeta
        if (auraShotgun is not null)
        {
            auraShotgun.Stop();
        }

        transform.parent = t_ma;
        // sistema local
        if (T_referencia_mano is not null)
        {
            // establecer posición y rotacion de la escopeta correctos (respecto a la mano)
            transform.localPosition = T_referencia_mano.localPosition;
            transform.localRotation = T_referencia_mano.localRotation;
        }
        // si no encuentra el transform de la orientación  correcta de la mano pone valores por defecto
        else
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public bool Carraga(int i_numero_projectils)
    {
        throw new System.NotImplementedException();
    }

    public bool Apuntar()
    {
        _isAiming = true;
        return true;
    }
    
    public bool CancelarApuntar()
    {
        _isAiming = false;
        return true;
    }

    void Update()
    {
        Debug.Log("Shotgun: " + cadencia + " segundos y " + damagePorPerdigon + " por perdigon");
    }

    void LateUpdate()
    {
        if (_textCanvas is null || camera is null || T_aimCamera is null) return;
        
        // Posición siempre globalmente encima de la escopeta
        _textCanvas.transform.position = transform.position + textOffset;
        // El texto  de la escopeta siempre mira a la cámara
        _textCanvas.transform.LookAt(camera);
        _textCanvas.transform.Rotate(0, -90, 0);

        Transform target = _isAiming ? T_aimCamera : T_normalCamera;
        movimentCamara.t_personatge_a_seguir = target;
        //camera.parent.transform.GetComponent<Moviment_Camara>().t_personatge_a_seguir = target;
        //camera.localPosition = Vector3.Lerp(camera.localPosition, target.localPosition, speedCamera * Time.deltaTime); 
    }
    
    [Header("Características Escopeta")]
    [SerializeField] private int numPerdigones;
    public int NumPerdigones => numPerdigones;
    [SerializeField] private float damagePorPerdigon;
    [SerializeField] private float dispersion;
    [SerializeField] private float alcance;
    [SerializeField] private GameObject efecto;
    [SerializeField] private float cadencia; // cooldown escopeta
    private PlayerStats_UI _playerStatsUI;
    
    private float tiempoSiguienteDisparo;
    public bool Disparar()
    {
        if (Time.time >= tiempoSiguienteDisparo)
        {
            if (efecto is not null)
            {
                Instantiate(efecto, T_efecto.position, T_efecto.rotation);
            }
        
            for (int i = 0; i < numPerdigones; i++)
            {
                Vector3 direccion = camera.forward + Random.insideUnitSphere * dispersion;
                Vector3 origen = camera.position + camera.forward;
                RaycastHit hit;
            
                bool impacta = Physics.Raycast(origen, direccion, out hit, alcance);
                if (impacta)
                {
                    // Para ver el raycast
                    Debug.DrawLine(origen, hit.point, Color.green, 200.0f);
                    Debug.Log("Perdigón " + i + " ha impactado con " + hit.collider.name);
                
                    // Si le impacta a una enemigo
                    Enemigo enemigo = hit.collider.GetComponentInParent<Enemigo>();
                    if (enemigo is not null) {
                        enemigo.ProcesarImpacto(hit, damagePorPerdigon);
                    }
                }
                else
                {
                    Debug.DrawRay(origen, direccion * alcance, Color.red, 200.0f);
                }
            }
            tiempoSiguienteDisparo = Time.time + cadencia;
            return true;
        }
        return false;
    }

    public void Soltar()
    {
        MostrarTexto();
        transform.parent = null;
        // Activar el Box Collider para que no atraviese el suelo
        if (coll_solido is not null)
        {
            coll_solido.enabled = true;
        }

        // Recuperar fisicas del RigidBody para que se caiga al suelo al soltarla
        if (rb is not null)
        {
            rb.isKinematic = false;
           
        }
        
        // Activar aura de la escopeta
        if (auraShotgun is not null)
        {
            auraShotgun.Play();
        }
    }
    
    private void ActualizarDamage()
    {
        damagePorPerdigon = _playerStatsUI.DamagePerdigon;
    }
    
    private void ActualizarCadencia()
    {
        cadencia = _playerStatsUI.Cadencia;
    }
    
    public void MostrarTexto()
    {
        if (_textCanvas is not null && Touchscreen.current is null)
        {
            _textCanvas.SetActive(true);   
        }
    }

    public void OcultarTexto()
    {
        if (_textCanvas is not null)
        {
            _textCanvas.SetActive(false);
        }
    }


    private void OnEnable()
    {
        PlayerStats_UI.OnShotgunUpgraded += ActualizarDamage;
        PlayerStats_UI.OnShotgunUpgraded += ActualizarCadencia;
    }
    
    private void OnDisable()
    {
        PlayerStats_UI.OnShotgunUpgraded -= ActualizarDamage;
        PlayerStats_UI.OnShotgunUpgraded -= ActualizarCadencia;

    }
}