using UnityEngine;

public sealed class RangedEnemy : EnemyAIBase
{
    [SerializeField] private Gun _gun;
    [SerializeField] private float _shootRange = 10f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _shootPoint;
    private float _shootTimer = 0f;

    protected override void Update()
    {
        base.Update();
        if (!CheckForPlayer())
        {
            _gun.StopFire();
        }
    }

    protected override void HandleChase()
    {
        base.HandleChase();

        if (playerTransform == null)
        {
            _agent.isStopped = false;
            EnterPatrolState();
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= _shootRange && _playerInSight)
        {
            ShootProjectile();
            if (distanceToPlayer <= _shootRange / 2)
            {
                _agent.isStopped = false;
                Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
                float retreatDistance = 5f;

                Vector3 retreatPosition = transform.position + directionAwayFromPlayer * retreatDistance;

                _agent.SetDestination(retreatPosition);
            }
            else
            {
                _agent.isStopped = true;
            }
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    private void ShootProjectile()
    {
        if (_projectilePrefab != null && _shootPoint != null)
        {
            _gun.Use();
            Debug.Log("Ranged attack!");
        }
    }
}