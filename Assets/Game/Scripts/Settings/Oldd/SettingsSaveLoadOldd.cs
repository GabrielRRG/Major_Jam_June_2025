using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public sealed class SettingsSaveLoadOldd : MonoBehaviour
{
    public static SettingsSaveLoadOldd instance;
    public SettingsDataOldd SettingsDataOldd = new();
    public static event Action onSaveAll;
    public static event Action onLoadAll;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _resetButton;
    
    private const string  FILE_NAME = "settings.json";
    private string settingsPath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        settingsPath = Path.Combine(Application.persistentDataPath, FILE_NAME);
        LoadAll();
    }

    public void SaveAll()
    {
        string json = JsonConvert.SerializeObject(SettingsDataOldd, Formatting.Indented);
        File.WriteAllText(settingsPath, json);
        onSaveAll?.Invoke();
    }

    public void LoadAll()
    {
        if (!File.Exists(settingsPath)) return;
        string json = File.ReadAllText(settingsPath);
        SettingsDataOldd = JsonConvert.DeserializeObject<SettingsDataOldd>(json);
        onLoadAll?.Invoke();
    }
    public void ResetSettings()
    {
        SettingsDataOldd = new();
        SaveAll();
    }
}