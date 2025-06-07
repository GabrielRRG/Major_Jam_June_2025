using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _minLookDistance = 0.5f;
    [SerializeField] private Transform _headPoint;
    [SerializeField] private float _rayLength = 100f;

    private Vector3 _mouseWorldPoint;

    private void Awake()
    {
        if (_playerCamera == null)
            _playerCamera = Camera.main;
    }

    private void Update()
    {
        UpdateMouseWorldPoint();
        RotateTowardsMouse();
        DrawHeadRaycast();
    }

    private void UpdateMouseWorldPoint()
    {
        Ray cameraRay = _playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, _headPoint.position);

        if (groundPlane.Raycast(cameraRay, out float enter))
            _mouseWorldPoint = cameraRay.GetPoint(enter);
        else
            _mouseWorldPoint = cameraRay.GetPoint(_rayLength);
    }

    private void RotateTowardsMouse()
    {
        Vector3 lookDirection = _mouseWorldPoint - transform.position;
        lookDirection.y = 0;

        float distanceToCursor = lookDirection.magnitude;
        
        if (distanceToCursor < _minLookDistance)
            return;
        
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = targetRotation;
    }


    private void DrawHeadRaycast()
    {
        Vector3 direction = (_mouseWorldPoint - _headPoint.position).normalized;
        direction.y = 0;
        Debug.DrawRay(_headPoint.position, direction * _rayLength, Color.red);
    }
}