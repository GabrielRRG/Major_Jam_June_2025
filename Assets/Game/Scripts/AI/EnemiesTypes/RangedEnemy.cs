using UnityEngine;

public sealed class RangedEnemy : EnemyAIBase
{
    [SerializeField] private float _shootRange = 10f;
    [SerializeField] private float _shootCooldown = 2f;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _shootPoint;

    private float _shootTimer = 0f;

    protected override void HandleChase()
    {
        base.HandleChase();

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer <= _shootRange)
        {
            _agent.isStopped = true;
            _shootTimer += Time.deltaTime;
            if (_shootTimer >= _shootCooldown)
            {
                _shootTimer = 0f;
                ShootProjectile();
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
            Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.LookRotation(_playerTransform.position - _shootPoint.position));
            Debug.Log("Ranged attack!");
        }
    }
}