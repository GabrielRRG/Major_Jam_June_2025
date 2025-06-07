using UnityEngine;

public sealed class MeleeEnemy : EnemyAIBase
{
    [SerializeField] private MeleeWeapon _weapon;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 1f;

    private float _attackTimer = 0f;

    protected override void HandleChase()
    {
        base.HandleChase();
        
        if (_playerTransform == null)
        {
            EnterPatrolState();
            return;
        }
        
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer <= _attackRange)
        {
            _attackTimer += Time.deltaTime;
            if (_attackTimer >= _attackCooldown)
            {
                _attackTimer = 0f;
                AttackPlayer();
            }
        }
    }

    private void AttackPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHealth>().TakeDamage(_weapon.damage);
    }
}