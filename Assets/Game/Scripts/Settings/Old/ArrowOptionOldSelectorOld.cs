using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button), typeof(Button))]
public sealed class ArrowOptionOldSelectorOld : MonoBehaviour, ISettingOptionOld
{
    [SerializeField] private TMP_Text _displayText;
    [SerializeField] private string[] _options;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    private int _currentIndex;
    private int _defaultIndex;

    public event Action OnValueChangedExternally;

    private void Awake()
    {
        if (_options == null || _options.Length == 0)
        {
            enabled = false;
            return;
        }
        
        _defaultIndex = _currentIndex = Mathf.Clamp(_currentIndex, 0, _options.Length - 1);
        
        if (_leftButton != null)
            _leftButton.onClick.AddListener(OnLeftClicked);
        if (_rightButton != null)
            _rightButton.onClick.AddListener(OnRightClicked);

        UpdateDisplay();
    }

    private void OnDestroy()
    {
        if (_leftButton != null)
            _leftButton.onClick.RemoveListener(OnLeftClicked);
        if (_rightButton != null)
            _rightButton.onClick.RemoveListener(OnRightClicked);
    }

    private void OnLeftClicked()
    {
        if (_options.Length == 0) return;
        _currentIndex = (_currentIndex - 1 + _options.Length) % _options.Length;
        UpdateDisplay();
        OnValueChangedExternally?.Invoke();
    }

    private void OnRightClicked()
    {
        if (_options.Length == 0) return;
        _currentIndex = (_currentIndex + 1) % _options.Length;
        UpdateDisplay();
        OnValueChangedExternally?.Invoke();
    }

    private void UpdateDisplay()
    {
        if (_displayText != null && _options.Length > 0)
            _displayText.text = _options[_currentIndex];
    }
    public int GetSelectedIndex()
    {
        return _currentIndex;
    }
    public void SetIndex(int idx)
    {
        int clamped = Mathf.Clamp(idx, 0, _options.Length - 1);
        if (clamped == _currentIndex) return;
        _currentIndex = clamped;
        UpdateDisplay();
        OnValueChangedExternally?.Invoke();
    }
    public bool IsDefault()
    {
        return _currentIndex == _defaultIndex;
    }
    public void ResetToDefault()
    {
        SetIndex(_defaultIndex);
    }

    string ISettingOptionOld.Key => gameObject.name;

    bool ISettingOptionOld.IsDefault() => IsDefault();

    void ISettingOptionOld.ResetToDefault() => ResetToDefault();

    string ISettingOptionOld.GetValueAsString() => GetSelectedIndex().ToString();

    void ISettingOptionOld.SetValueFromString(string s)
    {
        if (int.TryParse(s, out int idx))
            SetIndex(idx);
    }

    event Action ISettingOptionOld.OnChanged
    {
        add => OnValueChangedExternally += value;
        remove => OnValueChangedExternally -= value;
    }
}
