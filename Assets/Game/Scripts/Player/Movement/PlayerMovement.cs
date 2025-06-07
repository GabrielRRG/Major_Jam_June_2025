using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;

    [SerializeField] private InputActionReference _move;
    public Animator animator;

    private Rigidbody _rigidbody;
    private Vector2 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _moveDirection = _move.action.ReadValue<Vector2>();
        
        Vector3 worldMove = new Vector3(_moveDirection.x, 0f, _moveDirection.y);
        Vector3 localMove = transform.InverseTransformDirection(worldMove);
        animator.SetFloat("XDirection", localMove.x);
        animator.SetFloat("YDirection", localMove.z);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(
            _moveDirection.x * moveSpeed,
            _rigidbody.linearVelocity.y,
            _moveDirection.y * moveSpeed
        );
        _rigidbody.linearVelocity = velocity;
    }
}