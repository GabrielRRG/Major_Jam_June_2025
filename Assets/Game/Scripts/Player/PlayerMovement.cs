using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private InputActionReference _move;
    private Rigidbody _rigidbody;
    private Vector2 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _moveDirection = _move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(_moveDirection.x * _moveSpeed, _rigidbody.linearVelocity.y, _moveDirection.y * _moveSpeed);
    }
}
