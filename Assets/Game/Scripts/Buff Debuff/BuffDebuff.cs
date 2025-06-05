using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "BuffDebuff", menuName = "Scriptable Objects/BuffDebuff")]
public abstract class BuffDebuff : ScriptableObject
{
    public string effectName;
    public Image effectIcon;
    public int effectDuration;

    public abstract void Apply(GameObject target);
    public abstract void Remove(GameObject target);

}
