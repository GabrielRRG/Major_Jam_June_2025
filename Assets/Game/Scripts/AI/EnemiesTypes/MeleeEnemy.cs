using UnityEngine;

public sealed class MeleeEnemy : EnemyAIBase
{
    [SerializeField] private MeleeWeapon _weapon;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 1f;
    
    private float _attackTimer = Mathf.Infinity;

    protected override void Start()
    {
        base.Start();
        _animator.SetBool("Knife", true);
    }

    protected override void HandleChase()
    {
        base.HandleChase();
        
        if (playerTransform == null)
        {
            EnterPatrolState();
            return;
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= _attackRange)
        {
            _agent.isStopped = true;
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackCooldown)
            {
                Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, directionToPlayer);
                if (dot > 0.7f)
                {
                    AttackPlayer();
                }
                _attackTimer = 0f;
            }
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    public void AttackPlayer()
    {
        _animator.SetTrigger("Attack");
        playerTransform.gameObject.GetComponent<CharacterHealth>().TakeDamage(_weapon.damage);
    }
}