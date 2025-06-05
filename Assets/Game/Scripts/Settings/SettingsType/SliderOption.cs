using UnityEngine;
using UnityEngine.UI;

public class SliderOption : OptionBase
{
    public float defaultValue;
    public Slider slider;
    public override void Load()
    {
        slider.value = PlayerPrefs.GetFloat(optionName + "_value", defaultValue);
        base.Load();
    }

    public override void LoadDefault()
    {
        slider.value = defaultValue;
        base.LoadDefault();
    }
}