using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public sealed class ArrowOptionSelector : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _optionLabel;
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;

    [Header("Settings")]
    [SerializeField] private string[] _options;
    [SerializeField] private int _defaultIndex = 0;

    public event System.Action OnValueChangedExternally;

    private int _currentIndex = 0;

    private void Awake()
    {
        _currentIndex = Mathf.Clamp(_defaultIndex, 0, _options.Length - 1);
        UpdateUI();
        _leftArrow.onClick.AddListener(SelectPrevious);
        _rightArrow.onClick.AddListener(SelectNext);
    }

    private void SelectPrevious()
    {
        _currentIndex = (_currentIndex - 1 + _options.Length) % _options.Length;
        UpdateUI();
        OnValueChangedExternally?.Invoke();
    }

    private void SelectNext()
    {
        _currentIndex = (_currentIndex + 1) % _options.Length;
        UpdateUI();
        OnValueChangedExternally?.Invoke();
    }

    private void UpdateUI()
    {
        _optionLabel.text = _options[_currentIndex];
        OnValueChangedExternally?.Invoke();
    }
    
    #region Events
    public int GetSelectedIndex()
    {
        return _currentIndex;
    }

    public void SetIndex(int index)
    {
        _currentIndex = Mathf.Clamp(index, 0, _options.Length - 1);
        UpdateUI();
    }

    public void ResetToDefault()
    {
        _currentIndex = Mathf.Clamp(_defaultIndex, 0, _options.Length - 1);
        UpdateUI();
    }
    
    public bool IsDefault()
    {
        return _currentIndex == _defaultIndex;
    }
    #endregion
}
