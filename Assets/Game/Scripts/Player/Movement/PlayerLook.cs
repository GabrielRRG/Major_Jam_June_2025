using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerLook : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _rotationSpeed = 15f;
    [SerializeField] private Transform _headPoint;
    [SerializeField] private float _rayLength = 100f;

    private Vector3 _mouseWorldPoint;

    private void Awake()
    {
        if (_playerCamera == null)
            _playerCamera = Camera.main;

        if (_headPoint == null)
        {
            GameObject headObj = new GameObject("HeadPoint");
            headObj.transform.SetParent(transform);
            headObj.transform.localPosition = Vector3.up * 0.6f;
            _headPoint = headObj.transform;
        }
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

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void DrawHeadRaycast()
    {
        Vector3 direction = (_mouseWorldPoint - _headPoint.position).normalized;
        direction.y = 0;
        Debug.DrawRay(_headPoint.position, direction * _rayLength, Color.red);
    }
}