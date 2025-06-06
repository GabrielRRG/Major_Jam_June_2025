using UnityEngine;
using UnityEngine.InputSystem;

public class AnimalBase : MonoBehaviour
{
    [SerializeField] protected AnimalFormData _animalFormData;
    [SerializeField] protected float _timeBetweenAttack = 1f;
    [SerializeField] protected InputActionReference _atack;
    [SerializeField] protected float _sphereRadius = 1.5f;
    [SerializeField] protected float _attackDistance = 1.5f;
    [SerializeField] private LayerMask _attackMask;

    protected GameObject _player;

    private float _attackCooldown = 0f;
    
    private int _initialHealth;
    private int _initialMaxHP;
    private float _initialMoveSpeed;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
        {
            Debug.LogError("Player not found!");
            enabled = false;
            return;
        }

        InitAnimalStats();
    }

    private void InitAnimalStats()
    {
        CharacterHealth characterHealth = _player.GetComponent<CharacterHealth>();
        PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();

        _initialMaxHP = characterHealth.maxHealth;
        _initialMoveSpeed = playerMovement.moveSpeed;
        
        characterHealth.health = Mathf.RoundToInt(characterHealth.health * ((float)_animalFormData.maxHP / characterHealth.maxHealth));
        characterHealth.maxHealth = _animalFormData.maxHP;
        playerMovement.moveSpeed = _animalFormData.moveSpeed;
    }

    private void RevertStats()
    {
        CharacterHealth characterHealth = _player.GetComponent<CharacterHealth>();
        PlayerMovement playerMovement = _player.GetComponent<PlayerMovement>();
        
        characterHealth.health = Mathf.RoundToInt(characterHealth.health * ((float)_initialMaxHP / characterHealth.maxHealth));
        characterHealth.maxHealth = _initialMaxHP;

        playerMovement.moveSpeed = _initialMoveSpeed;
    }

    private void OnEnable()
    {
        _atack.action.started += AttackAction;
    }

    private void OnDisable()
    {
        RevertStats();
        _atack.action.started -= AttackAction;
    }

    private void Update()
    {
        if (_attackCooldown > 0f)
            _attackCooldown -= Time.deltaTime;
    }

    private void AttackAction(InputAction.CallbackContext ctx)
    {
        if (_attackCooldown > 0f)
            return;

        Attack();
        _attackCooldown = _timeBetweenAttack;
    }

    protected virtual void Attack()
    {
        Vector3 attackOrigin = transform.position + transform.forward * _attackDistance;

        Collider[] hitColliders = Physics.OverlapSphere(attackOrigin, _sphereRadius, _attackMask);
        bool hitSomething = false;

        foreach (var hit in hitColliders)
        {
            CharacterHealth targetHealth = hit.GetComponent<CharacterHealth>();
            if (targetHealth != null)
            {
                hitSomething = true;
                Debug.Log("Attacked object with CharacterHealth: " + hit.name);
                targetHealth.TakeDamage(_animalFormData.damage);
            }
        }

        if (!hitSomething)
        {
            Debug.Log("Attack missed — no valid target in front.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3 attackOrigin = transform.position + transform.forward * _attackDistance;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin, _sphereRadius);
    }
}
