using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/DoubleHealth")]
public class DoubleHealth : BuffDebuff
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<CharacterHealth>().Health *= 2;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<CharacterHealth>().Health /= 2;
    }
}
