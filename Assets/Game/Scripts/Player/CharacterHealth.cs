using UnityEngine;
using UnityEngine.UI;

public sealed class CharacterHealth : MonoBehaviour , IDamagable
{
    public int MaxHealth = 100;
    [SerializeField] private ParticleSystem _deathEffectPrefab;
    [SerializeField] private Slider _healthSlider;
    private int _health = 100;

    private ParticleSystem _deathEffect;

    public int Health { get => _health; set => _health = value; }

    void Start()
    {
        _health = MaxHealth;
        if(_healthSlider) _healthSlider.value = _health;
        _deathEffect = Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        _deathEffect.transform.SetParent(gameObject.transform);
    }
    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_healthSlider) _healthSlider.value = _health;
        if (_deathEffect)
        {
            _deathEffect.Play(); 
        }
        if(_health <= 0) 
        { 
            Destroy(gameObject); 
        }
    }
}
