using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Gun : Tool
{
    [Header("References")]
    [SerializeField] private GunData _gunData;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private InputActionReference _reloadInput;

    private bool _isFiring = false;
    private bool _isReloading = false;
    private float _nextTimeToFire = 0f;
    private int ammoLeft = 0;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;

    private void Start()
    {
        ammoLeft = _gunData.magazineCap;
    }

    public override void Use()
    {
        _isFiring = true;
    }
    public void StopFire()
    {
        _isFiring = false;
    }

    public override void Update()
    {
        base.Update();
        if(_isFiring && Time.time >= _nextTimeToFire && ammoLeft != 0 && !_isReloading)
        {
            switch (_gunData.fireMode)
            {
                case FireMode.Single:
                    Shoot();
                    _isFiring = false; // Stop firing until next input
                    _nextTimeToFire = Time.time + _gunData.fireRate;
                    break;

                case FireMode.Auto:
                    Shoot();
                    _nextTimeToFire = Time.time + _gunData.fireRate;
                    break;
            }
        }
    }
    private void ReloadGun(InputAction.CallbackContext obj)
    {
        if(!isPossessed) { return; }
        _isReloading = true;
        ammoLeft = _gunData.magazineCap;
        Invoke(nameof(SetReloadingState), _gunData.reloadTime);
    }
    private void SetReloadingState()
    {
        _isReloading = false;
    }

    private void Shoot()
    {
        for (int i = 0; i < _gunData.bulletsPerShot; i++)
        {
            Vector3 spreadDir = transform.forward + Random.insideUnitSphere * _gunData.spread;
            GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.LookRotation(spreadDir));
            bullet.GetComponent<Bullet>().damage = _gunData.damage;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = spreadDir.normalized * 30;
        }
        ammoLeft -= _gunData.bulletsPerShot; //We subtract the amount of bullets used!
        if (_muzzleFlash) _muzzleFlash.Play();
    }

    private void OnEnable()
    {
        _reloadInput.action.started += ReloadGun;
    }
    private void OnDisable()
    {
        _reloadInput.action.started -= ReloadGun;
    }
}
