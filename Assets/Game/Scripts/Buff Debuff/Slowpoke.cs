using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/Slowpoke")]
public class Slowpoke : BuffDebuff
{
    [Header("Buff Settings")]
    public int multiplier = 2;
    public override void Apply(GameObject target)
    {
        if (target == null) return;
        target.GetComponent<PlayerMovement>().moveSpeed /= multiplier;
    }

    public override void Remove(GameObject target)
    {
        if (target == null) return;
        target.GetComponent<PlayerMovement>().moveSpeed *= multiplier;
    }
}
