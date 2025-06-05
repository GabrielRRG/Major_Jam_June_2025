using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/HalfAmmo")]
public class HalfAmmo : BuffDebuff
{
    public AllGuns allGunsSO;
    [Header("Buff Settings")]
    public int multiplier = 2;
    public override void Apply(GameObject target)
    {
        foreach (GunData gunData in allGunsSO.allGuns)
        {
            gunData.magazineCap /= multiplier;
        }
    }

    public override void Remove(GameObject target)
    {
        foreach (GunData gunData in allGunsSO.allGuns)
        {
            gunData.magazineCap *= multiplier;
        }
    }
}
