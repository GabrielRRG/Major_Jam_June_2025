using UnityEngine;

public enum FireMode { Single, Auto }

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    public string gunName;
    public FireMode fireMode = FireMode.Single;

    [Header("Shooting Stats")]
    public float fireRate = 0.2f; // Time between shots
    public float spread = 0.1f;
    public float reloadTime = 3f;

    [Header("Ammo")]
    public int bulletsPerShot = 1;
    public int magazineCap = 6; //Amount of bullets in a single magazine

    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
}
