using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsManager : MonoBehaviour
{
    //This script needs to be modified for SO
    
    [SerializeField] private ArrowOptionSelector[] _selectors;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _resetButton;

    private const string KEY_PREFIX = "Setting ";
    private int[] _initialIndices;

    private void Awake()
    {
        if (_selectors == null || _selectors.Length == 0)
            _selectors = FindObjectsByType<ArrowOptionSelector>(FindObjectsSortMode.None);

        _saveButton.onClick.AddListener(SaveAll);
        _resetButton.onClick.AddListener(ResetToDefaults);

        _saveButton.interactable = false;
        _resetButton.interactable = false;
    }

    private void Start()
    {
        LoadAll();
        
        _initialIndices = new int[_selectors.Length];
        for (int i = 0; i < _selectors.Length; i++)
            _initialIndices[i] = _selectors[i].GetSelectedIndex();
        
        foreach (var selector in _selectors)
            selector.OnValueChangedExternally += OnSettingChanged;
        
        CheckResetButton();
    }
    
    private void OnSettingChanged()
    {
        if (_initialIndices == null || _initialIndices.Length != _selectors.Length)
            return;

        CheckSaveButton();
        CheckResetButton();
    }
    private void LoadAll()
    {
        foreach (var selector in _selectors)
        {
            string key = KEY_PREFIX + selector.name;
            if (PlayerPrefs.HasKey(key))
            {
                int savedIndex = PlayerPrefs.GetInt(key);
                selector.SetIndex(savedIndex);
            }
        }

        Debug.Log("Settings loaded");
    }
    public void SaveAll()
    {
        for (int i = 0; i < _selectors.Length; i++)
        {
            var selector = _selectors[i];
            string key = KEY_PREFIX + selector.name;
            int idx = selector.GetSelectedIndex();
            PlayerPrefs.SetInt(key, idx);
            _initialIndices[i] = idx;
        }

        PlayerPrefs.Save();
        Debug.Log("Settings saved");

        DisableButton(_saveButton);
    }
    public void ResetToDefaults()
    {
        for (int i = 0; i < _selectors.Length; i++)
        {
            var selector = _selectors[i];
            if (selector.gameObject.activeInHierarchy)
                selector.ResetToDefault();
        }
        SaveAll();
        
        CheckResetButton();
        Debug.Log("Settings reset to defaults and saved");
    }
    public void CheckSaveButton()
    {
        if (_initialIndices == null || _initialIndices.Length != _selectors.Length)
            return;
        Debug.Log("Checking Save button");
        bool anyChanged = false;
        for (int i = 0; i < _selectors.Length; i++)
        {
            var selector = _selectors[i];
            if (selector.GetSelectedIndex() != _initialIndices[i])
            {
                anyChanged = true;
                break;
            }
        }

        if (anyChanged) EnableButton(_saveButton);
        else DisableButton(_saveButton);
    }
    public void CheckResetButton()
    {
        bool anyNonDefault = false;
        for (int i = 0; i < _selectors.Length; i++)
        {
            var selector = _selectors[i];
            if (selector.gameObject.activeInHierarchy && !selector.IsDefault())
            {
                anyNonDefault = true;
                break;
            }
        }

        if (anyNonDefault) EnableButton(_resetButton);
        else DisableButton(_resetButton);
    }

    private void EnableButton(Button button)  => button.interactable = true;
    private void DisableButton(Button button) => button.interactable = false;
}
