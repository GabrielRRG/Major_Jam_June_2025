using RadiantTools.AudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Gun : Tool
{
    [Header("References")]
    public GunData gunData;
    [SerializeField] private Transform _shootPos;
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

    private void Awake()
    {
        _ammoLeft = gunData.magazineCap;
        _inventoryGroup = GameObject.FindGameObjectWithTag("Inventory").GetComponent<CanvasGroup>();
        _gunImage = _inventoryGroup.transform.Find("GunImage").GetComponent<Image>();
        _ammoCountText = _inventoryGroup.transform.Find("AmmoCount").GetComponent<TMP_Text>();
    }

    public void ShowGunUI()
    {
        Animator animator = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().animator;
        if(GameObject.FindGameObjectWithTag("Backpack").transform.childCount > 0)
        {
            animator.SetBool("Gun", true);
        }
        switch (gunData.gunName)
        {
            case "Pistol":
            {
                animator.SetBool("Pistol", true);
            }
                break;
            case "Shotgun":
            {
                animator.SetBool("Pistol", false);
            }
                break;
            case "AssaultRifle":
            {
                animator.SetBool("Pistol", false);
            }
                break;
        }
        _inventoryGroup.alpha = 1;
        _gunImage.sprite = gunData.gunIcon;
        _ammoCountText.text = _ammoLeft + "/" + gunData.magazineCap;
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
                Invoke(nameof(SetReloadingState), gunData.reloadTime);
            }
        }
        if(_isFiring && Time.time >= _nextTimeToFire && _ammoLeft != 0 && !_isReloading)
        {
            switch (gunData.fireMode)
            {
                case FireMode.Single:
                    Shoot();
                    _isFiring = false; // Stop firing until next input
                    _nextTimeToFire = Time.time + gunData.fireRate;
                    break;

                case FireMode.Auto:
                    Shoot();
                    _nextTimeToFire = Time.time + gunData.fireRate;
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

        Invoke(nameof(SetReloadingState), gunData.reloadTime);
    }
    private void SetReloadingState()
    {
        _ammoLeft = gunData.magazineCap;
        _isReloading = false;
        if(!_enemyGun) _ammoCountText.text = _ammoLeft + "/" + gunData.magazineCap;
    }

    private void Shoot()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().animator.SetTrigger("Attack");
        for (int i = 0; i < gunData.bulletsPerShot; i++)
        {
            Vector3 spreadDir = transform.forward + Random.insideUnitSphere * gunData.spread;
            Bullet bullet = Instantiate(_bulletPrefab, _shootPos.position, Quaternion.LookRotation(spreadDir)).GetComponent<Bullet>();
            bullet.damage = gunData.damage;
            bullet.enemyBullet = _enemyGun;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = spreadDir.normalized * 30;
        }
        _ammoLeft -= gunData.bulletsPerShot; //We subtract the amount of bullets used!

        if (!_enemyGun)
        {
            AudioPlayer soundSFX = AudioManager.Instance.GetAudioPlayer("SoundSFX");
            soundSFX.PlayAudioOnce((SoundTypes)SoundTypes.ToObject(typeof(SoundTypes), Random.Range(3,5)));
            _ammoCountText.text = _ammoLeft + "/" + gunData.magazineCap;
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
