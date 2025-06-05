using RadiantTools.AudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : Tool
{
    [Header("References")]
    [SerializeField] private GunData _gunData;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private InputActionReference _reloadInput;
    [SerializeField] private bool _enemyGun;

    [Header("UI")]
    private CanvasGroup _inventoryGroup;
    private Image _gunImage;
    private TMP_Text _ammoCountText;


    private bool _isFiring = false;
    private bool _isReloading = false;
    private float _nextTimeToFire = 0f;
    private int _ammoLeft = 0;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;

    private void Start()
    {
        _ammoLeft = _gunData.magazineCap;
        _inventoryGroup = GameObject.FindGameObjectWithTag("Inventory").GetComponent<CanvasGroup>();
        _gunImage = _inventoryGroup.transform.Find("GunImage").GetComponent<Image>();
        _ammoCountText = _inventoryGroup.transform.Find("AmmoCount").GetComponent<TMP_Text>();
    }

    public void ShowGunUI()
    {
        _inventoryGroup.alpha = 1;
        _gunImage.sprite = _gunData.gunIcon;
        _ammoCountText.text = _ammoLeft + "/" + _gunData.magazineCap;
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
        if (_enemyGun)
        {
            if (_ammoLeft <= 0)
            {
                _isReloading = true;
                Invoke(nameof(SetReloadingState), _gunData.reloadTime);
            }
        }
        if(_isFiring && Time.time >= _nextTimeToFire && _ammoLeft != 0 && !_isReloading)
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
        if(!isPossessed && !_enemyGun) { return; }
        _isReloading = true;
        _ammoCountText.text = "Reloading";

        AudioPlayer soundSFX = AudioManager.Instance.GetAudioPlayer("SoundSFX");
        soundSFX.PlayAudioOnce(SoundTypes.Reload);

        Invoke(nameof(SetReloadingState), _gunData.reloadTime);
    }
    private void SetReloadingState()
    {
        _ammoLeft = _gunData.magazineCap;
        _isReloading = false;
        if(!_enemyGun) _ammoCountText.text = _ammoLeft + "/" + _gunData.magazineCap;
    }

    private void Shoot()
    {
        for (int i = 0; i < _gunData.bulletsPerShot; i++)
        {
            Vector3 spreadDir = transform.forward + Random.insideUnitSphere * _gunData.spread;
            Bullet bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.LookRotation(spreadDir)).GetComponent<Bullet>();
            bullet.damage = _gunData.damage;
            bullet.enemyBullet = _enemyGun;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = spreadDir.normalized * 30;
        }
        _ammoLeft -= _gunData.bulletsPerShot; //We subtract the amount of bullets used!

        if (!_enemyGun)
        {
            AudioPlayer soundSFX = AudioManager.Instance.GetAudioPlayer("SoundSFX");
            soundSFX.PlayAudioOnce((SoundTypes)SoundTypes.ToObject(typeof(SoundTypes), Random.Range(3,5)));
            _ammoCountText.text = _ammoLeft + "/" + _gunData.magazineCap;
        }
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
