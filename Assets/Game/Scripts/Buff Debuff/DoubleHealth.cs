using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/DoubleHealth")]
public class DoubleHealth : BuffDebuff
{
    public override void Apply(GameObject target)
    {
        if (target == null) return;
        target.GetComponent<CharacterHealth>().Health *= 2;
        target.GetComponent<CharacterHealth>().maxHealth *= 2;
    }

    public override void Remove(GameObject target)
    {
        if (target != null)
        {
            target.GetComponent<CharacterHealth>().Health /= 2;
            target.GetComponent<CharacterHealth>().maxHealth /= 2;
        }
    }
}