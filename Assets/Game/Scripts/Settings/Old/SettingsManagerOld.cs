using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public sealed class GenericEntry
{
    public string key;
    public string value;
}

public sealed class SettingsManagerOld : MonoBehaviour
{
    /*[Header("UI buttons")]
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _resetButton;
    private List<ISettingOptionOld> _options = new();
    private List<string> _initialValues = new();
    private readonly string _fileName = "settings.json";

    private void Awake()
    {
        var allBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var mb in allBehaviours)
        {
            if (mb is ISettingOptionOld opt)
                _options.Add(opt);
        }
        _saveButton.onClick.AddListener(SaveAll);
        _resetButton.onClick.AddListener(ResetToDefaults);

        _saveButton.interactable = false;
        _resetButton.interactable = false;
    }

    private void Start()
    {
        LoadAll();
        _initialValues.Clear();
        foreach (var opt in _options)
            _initialValues.Add(opt.GetValueAsString());
        foreach (var opt in _options)
            opt.OnChanged += OnSettingChanged;

        CheckResetButton();
    }

    private void OnSettingChanged()
    {
        CheckSaveButton();
        CheckResetButton();
    }

    private void LoadAll()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, _fileName);
        if (!File.Exists(fullPath))
        {
            foreach (var opt in _options)
                opt.ResetToDefault();
            Debug.Log("Settings file not found. All options set to default.");
            return;
        }

        try
        {
            string json = File.ReadAllText(fullPath);
            var data = JsonUtility.FromJson<SettingsData>(json);
            
            foreach (var opt in _options)
                opt.ResetToDefault();
            
            if (data.entries != null)
            {
                foreach (var entry in data.entries)
                {
                    foreach (var opt in _options)
                    {
                        if (opt.Key == entry.key)
                        {
                            opt.SetValueFromString(entry.value);
                            break;
                        }
                    }
                }
            }

            Debug.Log("Settings loaded from JSON.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load settings from JSON: {e.Message}");
            foreach (var opt in _options)
                opt.ResetToDefault();
        }
    }

    public void SaveAll()
    {
        var entries = new List<GenericEntry>(_options.Count);
        _initialValues.Clear();
        
        foreach (var opt in _options)
        {
            string val = opt.GetValueAsString();
            entries.Add(new GenericEntry { key = opt.Key, value = val });
            _initialValues.Add(val);
        }

        var dataToSave = new SettingsData { entries = entries.ToArray() };

        try
        {
            string json = JsonUtility.ToJson(dataToSave, prettyPrint: true);
            string fullPath = Path.Combine(Application.persistentDataPath, _fileName);
            File.WriteAllText(fullPath, json);
            Debug.Log($"Settings saved to JSON: {fullPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save settings to JSON: {e.Message}");
        }

        _saveButton.interactable = false;
    }

    public void ResetToDefaults()
    {
        foreach (var opt in _options)
            opt.ResetToDefault();
        
        SaveAll();
        CheckResetButton();
        Debug.Log("Settings reset to defaults and saved.");
    }

    public void CheckSaveButton()
    {
        bool anyChanged = false;
        for (int i = 0; i < _options.Count; i++)
        {
            if (_options[i].GetValueAsString() != _initialValues[i])
            {
                anyChanged = true;
                break;
            }
        }
        _saveButton.interactable = anyChanged;
    }

    public void CheckResetButton()
    {
        bool anyNonDefault = false;
        foreach (var opt in _options)
        {
            if (!opt.IsDefault())
            {
                anyNonDefault = true;
                break;
            }
        }
        _resetButton.interactable = anyNonDefault;
    }*/
}
