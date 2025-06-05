using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/DoubleDamage")]
public class DoubleDamage : BuffDebuff
{
    public AllGuns allGunsSO;
    [Header("Buff Settings")]
    public int multiplier = 2;
    public override void Apply(GameObject target)
    {
        foreach(GunData gunData in allGunsSO.allGuns)
        {
            gunData.damage *= multiplier;
        }
    }

    public override void Remove(GameObject target)
    {
        foreach (GunData gunData in allGunsSO.allGuns)
        {
            gunData.damage /= multiplier;
        }
    }
}
