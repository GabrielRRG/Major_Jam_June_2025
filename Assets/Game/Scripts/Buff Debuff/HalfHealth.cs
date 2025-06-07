using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/HalfHealth")]
public class HalfHealth : BuffDebuff
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<CharacterHealth>().Health /= 2;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<CharacterHealth>().Health *= 2;
    }
}