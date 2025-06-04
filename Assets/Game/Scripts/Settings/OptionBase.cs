using UnityEngine;

public class OptionBase : MonoBehaviour
{
    public string optionName;

    public virtual void Load() {}

    public virtual void LoadDefault() {}
}