using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/HalfAmmo")]
public class HalfAmmo : BuffDebuff
{
    public override void Apply(GameObject target)
    {
        if (target == null) return;
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            gun.magazineSize /= 2;
            if(gun.ammoLeft > gun.magazineSize) gun.ammoLeft = gun.magazineSize;
        }
    }

    public override void Remove(GameObject target)
    {
        if (target == null) return;
        foreach (Gun gun in target.GetComponentsInChildren<Gun>())
        {
            gun.magazineSize = gun.gunData.magazineCap;
            if(gun.ammoLeft > gun.magazineSize) gun.ammoLeft = gun.magazineSize;
        }
    }
}
