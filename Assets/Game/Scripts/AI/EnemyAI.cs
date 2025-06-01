using UnityEngine;
using UnityEngine.AI;

public sealed class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("A list of patrol points (which AI will walk on).")]
    [SerializeField] private Transform[] _patrolPoints;
    [Tooltip("How many seconds to wait at each point before moving on.")]
    [SerializeField] private float _patrolWaitTime = 2f;
    
    [Header("Vision Settings")]
    [Tooltip("Maximum detection distance of the player.")]
    [SerializeField] private float _viewDistance = 10f;
    [Tooltip("Semi-horizontal angle of the field of view (in degrees). For example, 45 => FOV = 90°.")]
    [SerializeField] private float _viewAngle = 45f;
    [Tooltip("Layers that are considered obstacles (walls, etc.).")]
    [SerializeField] private LayerMask _obstacleMask;
    [Tooltip("Player layer (to quickly find the collider player).")]
    [SerializeField] private LayerMask _playerMask;
    
    [Header("Chase/ Search Settings")]
    [Tooltip("Movement speed when chasing a player.")]
    [SerializeField] private float _chaseSpeed = 5f;
    [Tooltip("Speed of movement when patrolling.")]
    [SerializeField] private float _patrolSpeed = 2f;
    [Tooltip("Pause at the location where the player was last detected before returning to patrolling.")]
    [SerializeField] private float _searchWaitTime = 3f;

    private NavMeshAgent _agent;
    private Transform _playerTransform;
    private EnemyState _currentState = EnemyState.Patrol;
    private int _currentPatrolIndex = 0;
    private float _waitTimer = 0f;               
    private Vector3 _lastKnownPlayerPos;         
    private bool _playerInSight = false;
    
    private enum EnemyState
    {
        Patrol,     
        Chase,      
        Search      
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _playerTransform = playerObj.transform;
        else
            Debug.LogError("Player object not found. Убедитесь, что у игрока есть тег 'Player'.");
        
        _currentState = EnemyState.Patrol;
        _agent.speed = _patrolSpeed;
        if (_patrolPoints.Length > 0)
        {
            _agent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
        }
    }

    void Update()
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

    // ------------------- PATROL -------------------
    private void HandlePatrol()
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

    // ------------------- PLAYER PURSUIT -------------------
    private void HandleChase()
    {
        if (_playerTransform == null)
            return;
        
        _agent.SetDestination(_playerTransform.position);
        
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

    // ------------------- LAST KNOWN POINT SEARCH -------------------
    private void HandleSearch()
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

    // ------------------- SWITCHING FUNCTIONS -------------------
    private void EnterPatrolState()
    {
        _currentState = EnemyState.Patrol;
        _agent.speed = _patrolSpeed;
        if (_patrolPoints.Length > 0)
        {
            _agent.SetDestination(_patrolPoints[_currentPatrolIndex].position);
        }
    }

    private void EnterChaseState()
    {
        _currentState = EnemyState.Chase;
        _agent.speed = _chaseSpeed;
        _playerInSight = true;
    }

    private void EnterSearchState()
    {
        _currentState = EnemyState.Search;
        _agent.speed = _chaseSpeed;
        _waitTimer = 0f;
        _agent.SetDestination(_lastKnownPlayerPos);
    }

    // ------------------- CHECK FOR DETECTION AND CONTINUED VISIBILITY -------------------
    private bool CheckForPlayer()
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
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, _viewDistance, ~0, QueryTriggerInteraction.Ignore))
        {
            return ((1 << hit.collider.gameObject.layer) & _playerMask) != 0;
        }

        return false;
    }
    private bool PlayerStillVisible()
    {
        return CheckForPlayer();
    }

    // ------------------- VISUALISATION IN THE FIELD (GIZMOS) -------------------
    private void OnDrawGizmosSelected()
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
