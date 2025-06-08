using UnityEngine;
using UnityEngine.UI;

public sealed class CharacterHealth : MonoBehaviour , IDamagable
{
    public int maxHealth = 100;
    private int health = 100;
    public Transform buffsFolder;
    [SerializeField] private ParticleSystem _deathEffectPrefab;
    [SerializeField] private Slider _healthSlider;

    private ParticleSystem _deathEffect;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            UpdateSlider();

            if (health > 0) return;
            if (CompareTag("Player"))
            {
                GameObject.FindGameObjectWithTag("Inventory").GetComponent<CanvasGroup>().alpha = 0;
                GameObject.FindGameObjectWithTag("UIBackground").GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        health = maxHealth;
        _healthSlider.value = (float)Health / maxHealth;
        _deathEffect = Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        _deathEffect.transform.SetParent(gameObject.transform);
    }
    public void UpdateSlider()
    {
        if (_healthSlider) _healthSlider.value = (float)Health / maxHealth;
    }
    public void TakeDamage(int amount)
    {
        Health -= amount;

        if (CompareTag("Enemy"))
        {
            EnemyAIBase enemyAI = GetComponent<EnemyAIBase>();
            enemyAI.roomManager.SetAlarm(enemyAI.playerTransform.position);
        }
        
        if (_deathEffect)
            _deathEffect.Play();
    }
}
