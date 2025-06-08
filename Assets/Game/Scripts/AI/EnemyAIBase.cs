using UnityEngine;
using UnityEngine.AI;

public class EnemyAIBase : MonoBehaviour
{
    public EnemyRoomManager roomManager;

    [Header("Patrol Settings")] [Tooltip("A list of patrol points (which AI will walk on).")] [SerializeField]
    protected Transform[] _patrolPoints;

    [Tooltip("How many seconds to wait at each point before moving on.")] [SerializeField]
    protected float _patrolWaitTime = 2f;

    [Header("Vision Settings")] [Tooltip("Maximum detection distance of the player.")] [SerializeField]
    protected float _viewDistance = 10f;

    [Tooltip("Semi-horizontal angle of the field of view (in degrees). For example, 45 => FOV = 90°.")] [SerializeField]
    protected float _viewAngle = 45f;

    [Tooltip("Layers that are considered obstacles (walls, etc.).")] [SerializeField]
    protected LayerMask _obstacleMask;

    [Tooltip("Player layer (to quickly find the collider player).")] [SerializeField]
    protected LayerMask _playerMask;

    [Header("Chase/ Search Settings")] [Tooltip("Movement speed when chasing a player.")] [SerializeField]
    protected float _chaseSpeed = 5f;

    [Tooltip("Speed of movement when patrolling.")] [SerializeField]
    protected float _patrolSpeed = 2f;

    [Tooltip("Pause at the location where the player was last detected before returning to patrolling.")]
    [SerializeField]
    protected float _searchWaitTime = 3f;

    public Transform playerTransform;
    protected Vector3 lastKnownPlayerPos;
    protected NavMeshAgent _agent;
    protected EnemyState _currentState = EnemyState.Patrol;
    protected int _currentPatrolIndex = 0;
    protected float _waitTimer = 0f;
    protected bool _playerInSight = false;
    protected Animator _animator;

    protected enum EnemyState
    {
        Patrol,
        Chase,
        Search
    }

    protected virtual void Start()
    {
        _animator = gameObject.GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
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
        
        Vector3 moveDirection = _agent.desiredVelocity.normalized;
        Vector3 worldMove = new Vector3(moveDirection.x, 0f, moveDirection.z);
        Vector3 localMove = transform.InverseTransformDirection(worldMove);
        _animator.SetFloat("XDirection", localMove.x, dampTime: 0.1f, deltaTime: Time.deltaTime);
        _animator.SetFloat("YDirection", localMove.z, dampTime: 0.1f, deltaTime: Time.deltaTime);
    }

    // --- PATROL ---
    protected virtual void HandlePatrol()
    {
        if (_patrolPoints.Length == 0)
            return;

        RotateTowards(_patrolPoints[_currentPatrolIndex].position);
        if (!_agent.pathPending && _agent.remainingDistance < 1.1f)
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
        if(playerTransform == null) return;
        _agent.SetDestination(lastKnownPlayerPos);

        RotateTowards(lastKnownPlayerPos);

        if (!PlayerStillVisible())
        {
            EnterSearchState();
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15);
        }
    }


    // --- SEARCH ---
    protected virtual void HandleSearch()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 1.1f)
        {
            RotateTowards(lastKnownPlayerPos);

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
        Debug.Log("Entering chase state");
        _currentState = EnemyState.Chase;
        _agent.speed = _chaseSpeed;
    }

    protected virtual void EnterSearchState()
    {
        _currentState = EnemyState.Search;
        _agent.speed = _chaseSpeed;
        _waitTimer = 0f;
        _agent.SetDestination(lastKnownPlayerPos);
    }

    // --- CHECK PLAYER DETECTION ---
    protected virtual bool CheckForPlayer()
    {
        if (playerTransform == null)
            return false;

        Vector3 directionToPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        float closeRadius = 2f;
        Collider[] hits = Physics.OverlapSphere(transform.position, closeRadius, _playerMask);
        foreach (var currentHit in hits)
        {
            Vector3 origin = transform.position + Vector3.up * 1.5f;
            Vector3 target = currentHit.transform.position + Vector3.up * 0.5f;
            Vector3 dir = (target - origin).normalized;
            float dist = Vector3.Distance(origin, target);

            if (!Physics.Raycast(origin, dir, dist, _obstacleMask))
            {
                Debug.Log("Player detected in close range");
                _playerInSight = true;
                roomManager.SetAlarm(playerTransform.position);
            }
            else
            {
                _playerInSight = false;
            }
        }
        if (distanceToPlayer <= _viewDistance && angleToPlayer <= _viewAngle)
        {
            Vector3 eye = transform.position + Vector3.up * 1.5f;
            Vector3 targetPoint = playerTransform.position + Vector3.up * 0.5f;
            Vector3 rayDir = (targetPoint - eye).normalized;

            if (Physics.Raycast(eye, rayDir, out RaycastHit hit, _viewDistance, _playerMask))
            {
                float distToPlayer = Vector3.Distance(eye, hit.point);

                if (!Physics.Raycast(eye, rayDir, distToPlayer, _obstacleMask))
                {
                    _playerInSight = true;
                    roomManager.SetAlarm(playerTransform.position);
                }
                else
                {
                    _playerInSight = false;
                }
            }
            else
            {
                _playerInSight = false;
            }
        }
        else
        {
            _playerInSight = false;
        }

        if (_playerInSight)
        {
            lastKnownPlayerPos = playerTransform.position;
            return true;
        }

        if (roomManager.alarm)
        {
            lastKnownPlayerPos = roomManager.alarmPosition;
            return true;
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
        Gizmos.DrawLine(lastKnownPlayerPos + Vector3.up * 0.5f, lastKnownPlayerPos + Vector3.down * 0.5f);
        Gizmos.DrawLine(lastKnownPlayerPos + Vector3.left * 0.5f, lastKnownPlayerPos + Vector3.right * 0.5f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}