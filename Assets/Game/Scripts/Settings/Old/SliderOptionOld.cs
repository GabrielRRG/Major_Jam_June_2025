using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public sealed class SliderOptionOld : MonoBehaviour, ISettingOptionOld
{
    private Slider _slider;
    private float  _defaultValue;
    public event Action OnChanged;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _defaultValue = _slider.value;
        _slider.onValueChanged.AddListener(value => OnChanged?.Invoke());
    }
    public string Key => gameObject.name;
    bool ISettingOptionOld.IsDefault()
    {
        return Mathf.Approximately(_slider.value, _defaultValue);
    }
    void ISettingOptionOld.ResetToDefault()
    {
        _slider.value = _defaultValue;
    }
    string ISettingOptionOld.GetValueAsString()
    {
        return _slider.value.ToString("R");
    }
    void ISettingOptionOld.SetValueFromString(string s)
    {
        if (float.TryParse(s, out float v))
            _slider.value = v;
    }
}