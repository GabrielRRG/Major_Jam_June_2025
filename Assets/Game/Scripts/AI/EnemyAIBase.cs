using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBase : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("A list of patrol points (which AI will walk on).")]
    [SerializeField] protected Transform[] _patrolPoints;
    [Tooltip("How many seconds to wait at each point before moving on.")]
    [SerializeField] protected float _patrolWaitTime = 2f;

    [Header("Vision Settings")]
    [Tooltip("Maximum detection distance of the player.")]
    [SerializeField] protected float _viewDistance = 10f;
    [Tooltip("Semi-horizontal angle of the field of view (in degrees). For example, 45 => FOV = 90°.")]
    [SerializeField] protected float _viewAngle = 45f;
    [Tooltip("Layers that are considered obstacles (walls, etc.).")]
    [SerializeField] protected LayerMask _obstacleMask;
    [Tooltip("Player layer (to quickly find the collider player).")]
    [SerializeField] protected LayerMask _playerMask;

    [Header("Chase/ Search Settings")]
    [Tooltip("Movement speed when chasing a player.")]
    [SerializeField] protected float _chaseSpeed = 5f;
    [Tooltip("Speed of movement when patrolling.")]
    [SerializeField] protected float _patrolSpeed = 2f;
    [Tooltip("Pause at the location where the player was last detected before returning to patrolling.")]
    [SerializeField] protected float _searchWaitTime = 3f;

    protected NavMeshAgent _agent;
    protected Transform _playerTransform;
    protected EnemyState _currentState = EnemyState.Patrol;
    protected int _currentPatrolIndex = 0;
    protected float _waitTimer = 0f;
    protected Vector3 _lastKnownPlayerPos;
    protected bool _playerInSight = false;

    protected enum EnemyState
    {
        Patrol,
        Chase,
        Search
    }

    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _playerTransform = playerObj.transform;
        else
            Debug.LogError("Player object not found. Make sure the player has the tag 'Player'.");
        _currentState = EnemyState.Patrol;
        _agent.speed = _patrolSpeed;

        if (_patrolPoints.Length > 0)
        {
            _agent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
        }
    }

    protected virtual void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                if (CheckForPlayer())
                {
                    EnterChaseState();
                }
                break;

            case EnemyState.Chase:
                HandleChase();
                break;

            case EnemyState.Search:
                HandleSearch();
                if (CheckForPlayer())
                {
                    EnterChaseState();
                }
                break;
        }
    }

    // --- PATROL ---
    protected virtual void HandlePatrol()
    {
        if (_patrolPoints.Length == 0)
            return;

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _patrolWaitTime)
            {
                _waitTimer = 0f;
                _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Length;
                _agent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
            }
        }
    }

    // --- CHASE ---
    protected virtual void HandleChase()
    {
        if (_playerTransform == null)
            return;

        _agent.SetDestination(_playerTransform.position);
        
        RotateTowardsPlayer();

        if (PlayerStillVisible())
        {
            _lastKnownPlayerPos = _playerTransform.position;
        }
        else
        {
            _lastKnownPlayerPos = _playerTransform.position;
            EnterSearchState();
        }
    }
    
    private void RotateTowardsPlayer()
    {
        if (_playerTransform == null)
            return;

        Vector3 direction = _playerTransform.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }


    // --- SEARCH ---
    protected virtual void HandleSearch()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _searchWaitTime)
            {
                _waitTimer = 0f;
                EnterPatrolState();
            }
        }
    }

    // --- STATES SWITCHING ---
    protected virtual void EnterPatrolState()
    {
        _currentState = EnemyState.Patrol;
        _agent.speed = _patrolSpeed;
        if (_patrolPoints.Length > 0)
        {
            _agent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
        }
    }

    protected virtual void EnterChaseState()
    {
        _currentState = EnemyState.Chase;
        _agent.speed = _chaseSpeed;
        _playerInSight = true;
    }

    protected virtual void EnterSearchState()
    {
        _currentState = EnemyState.Search;
        _agent.speed = _chaseSpeed;
        _waitTimer = 0f;
        _agent.SetDestination(_lastKnownPlayerPos);
    }

    // --- CHECK PLAYER DETECTION ---
    protected virtual bool CheckForPlayer()
    {
        if (_playerTransform == null)
            return false;

        Vector3 directionToPlayer = _playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > _viewDistance)
            return false;

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > _viewAngle)
            return false;

        Ray ray = new Ray(transform.position, directionToPlayer.normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, _viewDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            return ((1 << hit.collider.gameObject.layer) & _playerMask) != 0;
        }

        return false;
    }

    protected virtual bool PlayerStillVisible()
    {
        return CheckForPlayer();
    }

    // --- GIZMOS ---
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -_viewAngle, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, _viewAngle, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftBoundary * _viewDistance);
        Gizmos.DrawRay(transform.position, rightBoundary * _viewDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_lastKnownPlayerPos + Vector3.up * 0.5f, _lastKnownPlayerPos + Vector3.down * 0.5f);
        Gizmos.DrawLine(_lastKnownPlayerPos + Vector3.left * 0.5f, _lastKnownPlayerPos + Vector3.right * 0.5f);
    }
}
