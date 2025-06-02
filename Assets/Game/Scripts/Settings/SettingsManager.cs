using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsManager : MonoBehaviour
{
    //This script needs to be modified for SO
    
    [SerializeField] private ArrowOptionSelector[] _selectors;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _resetButton;

    private const string KEY_PREFIX = "Setting ";

    // Stores the initial indices loaded from PlayerPrefs
    private int[] _initialIndices;

    private void Awake()
    {
        // If no selectors are set in the inspector, find them all in the scene
        if (_selectors == null || _selectors.Length == 0)
            _selectors = FindObjectsByType<ArrowOptionSelector>(FindObjectsSortMode.None);

        _saveButton.onClick.AddListener(SaveAll);
        _resetButton.onClick.AddListener(ResetToDefaults);

        _saveButton.interactable = false;
        _resetButton.interactable = false;
    }

    private void Start()
    {
        // 1) Let each ArrowOptionSelector initialize its own defaultIndex
        //    (its Start() runs before this Start())

        // 2) Now load saved values from PlayerPrefs
        LoadAll();

        // 3) Capture “initial” indices right after loading
        _initialIndices = new int[_selectors.Length];
        for (int i = 0; i < _selectors.Length; i++)
            _initialIndices[i] = _selectors[i].GetSelectedIndex();

        // 4) Subscribe to changes only after _initialIndices is set
        foreach (var selector in _selectors)
            selector.OnValueChangedExternally += OnSettingChanged;

        // 5) Finally, check if resetButton should be enabled at launch
        CheckResetButton();
    }

    // Called when any selector’s value changes
    private void OnSettingChanged()
    {
        // If initialIndices is not yet set, skip
        if (_initialIndices == null || _initialIndices.Length != _selectors.Length)
            return;

        CheckSaveButton();
        CheckResetButton();
    }

    // Load saved indices from PlayerPrefs (if they exist)
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

    // Save current indices of all selectors into PlayerPrefs
    public void SaveAll()
    {
        for (int i = 0; i < _selectors.Length; i++)
        {
            var selector = _selectors[i];
            string key = KEY_PREFIX + selector.name;
            int idx = selector.GetSelectedIndex();
            PlayerPrefs.SetInt(key, idx);

            // Update the “initial” index after saving
            _initialIndices[i] = idx;
        }

        PlayerPrefs.Save();
        Debug.Log("Settings saved");

        DisableButton(_saveButton);
    }

    // Reset active selectors to their default and immediately save
    public void ResetToDefaults()
    {
        for (int i = 0; i < _selectors.Length; i++)
        {
            var selector = _selectors[i];
            if (selector.gameObject.activeInHierarchy)
                selector.ResetToDefault();
        }

        // Now immediately save those default values
        SaveAll();

        // After SaveAll(), “Save” is already disabled, so only check resetButton
        CheckResetButton();
        Debug.Log("Settings reset to defaults and saved");
    }

    // Enable or disable the Save button based on whether any active selector
    // differs from its initial (loaded) value
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

    // Enable or disable the Reset button based on whether any active selector
    // is not at its default value
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
