using UnityEngine;
using UnityEngine.UI;

public class Player_Healthbar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;
    private Canvas _canvas;
    private float _target;
    private float _reduceSpeed = 1f;
    private Color _red;
    private Color _green;
    
    private void Start()
    {
        _canvas = gameObject.GetComponent<Canvas>();
        _red = new Color(219/255f,55/255f,55/255f,255/255f);
        _green = new Color(35/255f,197/255f,63/255f,255/255f);
        healthbarSprite.color = _green;
    }

    public void UpdateHealthbar(float currentHealth, float maxHealth)
    {
        _target = currentHealth / maxHealth;
    }

    void Update()
    {
        if (_canvas is not null)
        {
            healthbarSprite.fillAmount = Mathf.MoveTowards(healthbarSprite.fillAmount, _target, _reduceSpeed * Time.deltaTime);
            healthbarSprite.color = Color.Lerp(healthbarSprite.color, Color.Lerp(_red, _green, healthbarSprite.fillAmount), _reduceSpeed * Time.deltaTime );
        }
    }
}
