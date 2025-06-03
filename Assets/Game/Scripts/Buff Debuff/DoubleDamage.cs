using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/DoubleDamage")]
public class DoubleDamage : BuffDebuff
{
    public GunData[] allGunData;
    public override void Apply(GameObject target)
    {
        foreach(GunData gunData in allGunData)
        {
            gunData.damage *= 2;
        }
    }

    public override void Remove(GameObject target)
    {
        foreach (GunData gunData in allGunData)
        {
            gunData.damage /= 2;
        }
    }
}
