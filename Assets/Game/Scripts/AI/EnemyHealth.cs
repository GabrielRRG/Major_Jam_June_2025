using UnityEngine;
using UnityEngine.UI;

public sealed class EnemyHealth : MonoBehaviour , IDamagable
{
    [SerializeField] private ParticleSystem _deathEffectPrefab;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private int _health = 100;

    private ParticleSystem _deathEffect;
    void Start()
    {
        _healthSlider.value = _health;
        _deathEffect = Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        _deathEffect.transform.SetParent(gameObject.transform);
    }
    public void TakeDamage(int amount)
    {
        _health -= amount;
        _healthSlider.value = _health;
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
