using UnityEngine;
using UnityEngine.Audio;

public class Gun : Tool
{
    [SerializeField] private GunData _gunData;
    [SerializeField] private GameObject bulletPrefab;
    public override void Use()
    {
        Shoot();
    }

    private void Shoot()
    {
        for (int i = 0; i < _gunData.bulletsPerShot; i++)
        {
            Vector3 spreadDir = transform.forward + Random.insideUnitSphere * _gunData.spread;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(spreadDir));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = spreadDir.normalized * 30;
        }

        if (_gunData.muzzleFlash) _gunData.muzzleFlash.Play();
        //if (_gunData.shootSound) audioSource.PlayOneShot(_gunData.shootSound);
    }
}
