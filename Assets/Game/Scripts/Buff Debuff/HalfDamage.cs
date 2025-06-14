using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/HalfDamage")]
public class HalfDamage : BuffDebuff
{
    int originalDamage;
    public override void Apply(GameObject target)
    {
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            originalDamage = gun.damage;
            gun.damage /= 2;
        }
    }

    public override void Remove(GameObject target)
    {
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            gun.damage = originalDamage;
        }
    }
}
