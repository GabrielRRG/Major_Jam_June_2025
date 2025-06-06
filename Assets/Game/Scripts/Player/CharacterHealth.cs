using UnityEngine;
using UnityEngine.UI;

public sealed class CharacterHealth : MonoBehaviour , IDamagable
{
    public int maxHealth = 100;
    public int health = 100;
    [SerializeField] private ParticleSystem _deathEffectPrefab;
    [SerializeField] private Slider _healthSlider;

    private ParticleSystem _deathEffect;

    public int Health { get => health; set => health = value; }

    void Start()
    {
        _healthSlider.value = (float)health / maxHealth;
        if(_healthSlider) _healthSlider.value = health;
        _deathEffect = Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        _deathEffect.transform.SetParent(gameObject.transform);
    }
    public void UpdateSlider()
    {
        if (_healthSlider) _healthSlider.value = health;
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (_healthSlider) _healthSlider.value = (float)health / maxHealth;
        if (_deathEffect)
        {
            _deathEffect.Play(); 
        }
        if(health <= 0) 
        { 
            if(gameObject.CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Inventory").GetComponent<CanvasGroup>().alpha = 0;
                GameObject.FindGameObjectWithTag("UIBackground").GetComponent<CanvasGroup>().alpha = 1;
                return;
            }
            else
            {
                Destroy(gameObject); 
            }
        }
    }
}
