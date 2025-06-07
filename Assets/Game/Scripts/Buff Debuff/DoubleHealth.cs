using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/DoubleHealth")]
public class DoubleHealth : BuffDebuff
{
    int originalHealth;
    public override void Apply(GameObject target)
    {
        originalHealth = target.GetComponent<CharacterHealth>().Health;
        target.GetComponent<CharacterHealth>().Health *= 2;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<CharacterHealth>().Health = originalHealth;
    }
}
