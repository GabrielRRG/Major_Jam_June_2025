using UnityEngine;

public sealed class MeleeEnemy : EnemyAIBase
{
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 1f;

    private float _attackTimer = 0f;

    protected override void HandleChase()
    {
        base.HandleChase();
        
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
        Debug.Log("Melee attack!");
    }
}