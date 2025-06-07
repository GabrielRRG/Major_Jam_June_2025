using RadiantTools.AudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MeleeWeapon : Tool
{
    [Header("References")] public MeleeWeaponData weaponData;
    [SerializeField] private bool _enemyWeapon;
    public int damage;

    [Header("UI")]
    private CanvasGroup _inventoryGroup;
    private Image _weaponImage;
    
    private bool _isFiring = false;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;

    private void Awake()
    {
        damage = weaponData.damage;
        _inventoryGroup = GameObject.FindGameObjectWithTag("Inventory").GetComponent<CanvasGroup>();
        _weaponImage = _inventoryGroup.transform.Find("GunImage").GetComponent<Image>();
    }

    public void ShowGunUI()
    {
        Animator animator = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().animator;
        switch (weaponData.weaponName)
        {
            case "Knife":
            {
                animator.SetBool("Knife", true);
            }
                break;
        }
        _inventoryGroup.alpha = 1;
        _weaponImage.sprite = weaponData.weaponIcon;
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
        Attack();
    }
    private void Attack()
    {
        if (!_enemyWeapon)
        {
            AudioPlayer soundSFX = AudioManager.Instance.GetAudioPlayer("SoundSFX");
            soundSFX.PlayAudioOnce((SoundTypes)SoundTypes.ToObject(typeof(SoundTypes), UnityEngine.Random.Range(3,5)));
        }
        if (_muzzleFlash) _muzzleFlash.Play();
    }
}