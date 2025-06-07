using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/DoubleAmmo")]
public class DoubleAmmo : BuffDebuff
{
    int originalAmmo;
    public override void Apply(GameObject target)
    {
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            originalAmmo = gun._magazineSize;
            gun._magazineSize *= 2;
        }
    }

    public override void Remove(GameObject target)
    {
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            gun._magazineSize = originalAmmo;
        }
    }
}
