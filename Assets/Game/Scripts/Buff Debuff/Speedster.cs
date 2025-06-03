using UnityEngine;

[CreateAssetMenu(menuName = "Buffs | Debuffs/Speedster")]
public class Speedster : BuffDebuff
{
    public int speedMultiplier = 2;
    public override void Apply(GameObject target)
    {
        target.GetComponent<PlayerMovement>().MoveSpeed *= speedMultiplier;
    }

    public override void Remove(GameObject target)
    {
        target.GetComponent<PlayerMovement>().MoveSpeed /= speedMultiplier;
    }
}
