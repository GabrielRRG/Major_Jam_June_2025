using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private InputActionReference _move;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _rotationSpeed = 15f;
    [SerializeField] private Transform _headPoint;
    [SerializeField] private float _rayLength = 100f;
    [SerializeField] private LayerMask _raycastMask = ~0;
    
    private Rigidbody _rigidbody;
    private Vector2 _moveDirection;
    private Vector3 _mouseWorldPoint;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_playerCamera == null)
            _playerCamera = Camera.main;
    }

    private void Update()
    {
        _moveDirection = _move.action.ReadValue<Vector2>();
        DrawHeadRaycast();
        UpdateMouseWorldPoint();
        RotateTowardsMouse();
    }

    private void UpdateMouseWorldPoint()
    {
        Ray cameraRay = _playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        if (Physics.Raycast(cameraRay, out RaycastHit hit, _rayLength, _raycastMask))
        {
            _mouseWorldPoint = hit.point;
        }
        else
        {
            Plane groundPlane = new Plane(Vector3.up, _headPoint.position);
            if (groundPlane.Raycast(cameraRay, out float enter))
            {
                _mouseWorldPoint = cameraRay.GetPoint(enter);
            }
            else
            {
                _mouseWorldPoint = cameraRay.GetPoint(_rayLength);
            }
        }
    }

    private void RotateTowardsMouse()
    {
        Vector3 lookDirection = _mouseWorldPoint - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }

    private void DrawHeadRaycast()
    {
        if (_headPoint == null)
        {
            GameObject headObj = new GameObject("HeadPoint");
            headObj.transform.SetParent(transform);
            headObj.transform.localPosition = Vector3.up * 0.6f;
            _headPoint = headObj.transform;
        }

        Vector3 direction = (_mouseWorldPoint - _headPoint.position).normalized;
        
        Debug.DrawRay(_headPoint.position, direction * _rayLength, Color.red);

        if (Physics.Raycast(_headPoint.position, direction, out RaycastHit hit, _rayLength, _raycastMask))
        {
            Debug.DrawLine(_headPoint.position, hit.point, Color.green);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = new Vector3(_moveDirection.x * _moveSpeed, _rigidbody.linearVelocity.y, _moveDirection.y * _moveSpeed);
    }

    private void OnDrawGizmosSelected()
    {
        if (_headPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_headPoint.position, 0.1f);
            Gizmos.DrawLine(_headPoint.position, _mouseWorldPoint);
        }
    }
}