using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/DoubleAmmo")]
public class DoubleAmmo : BuffDebuff
{
    public override void Apply(GameObject target)
    {
        if (target == null) return;
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            gun.magazineSize *= 2;
            if(gun.ammoLeft > gun.magazineSize) gun.ammoLeft = gun.magazineSize;
        }
    }

    public override void Remove(GameObject target)
    {
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            gun.magazineSize = gun.gunData.magazineCap;
            if(gun.ammoLeft > gun.magazineSize) gun.ammoLeft = gun.magazineSize;
        }
    }
}
