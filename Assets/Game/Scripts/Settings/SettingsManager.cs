using UnityEngine;
using Screen = UnityEngine.Screen;
using System;

public class SettingsManager : MonoBehaviour
{
    public Action SaveAll;
    [SerializeField] private GameObject[] settingMenuButtons;
    [Header("Options")]
    [SerializeField] private ArrowOption[] arrowOptions;
    [SerializeField] private SliderOption[] sliderOptions;
    [SerializeField] private KeyOption[] keyOptions;
    
    [Header("GameplayLD")]
    public int showHints;
    [Header("GraphicsLD")]
    public Resolution resolution;
    public int qualityLevel;
    [Header("AudioLD")]
    public float masterVolume;

    public static SettingsManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadSaves();
        ChangeMenu(settingMenuButtons[0]);
    }

    public void Save()
    {
        for (int i = 0; i < arrowOptions.Length; i++)
        {
            PlayerPrefs.SetInt(arrowOptions[i].optionName + "_index", arrowOptions[i].currentIndex);
        }

        for (int i = 0; i < sliderOptions.Length; i++)
        {
            PlayerPrefs.SetFloat(sliderOptions[i].optionName + "_value", sliderOptions[i].slider.value);
        }

        for (int i = 0; i < keyOptions.Length; i++)
        {
            keyOptions[i].Load();
        }
        PlayerPrefs.Save();
        ApplyLD();
        SaveAll?.Invoke();
    }

    public void LoadSaves()
    {
        for (int i = 0; i < arrowOptions.Length; i++)
        {
            arrowOptions[i].Load();
        }

        for (int i = 0; i < sliderOptions.Length; i++)
        {
            sliderOptions[i].Load();
        }

        for (int i = 0; i < keyOptions.Length; i++)
        {
            keyOptions[i].Load();
        }
        Save();
    }

    public void LoadDefaults()
    {
        for (int i = 0; i < arrowOptions.Length; i++)
        {
            arrowOptions[i].LoadDefault();
        }

        for (int i = 0; i < sliderOptions.Length; i++)
        {
            sliderOptions[i].LoadDefault();
        }

        for (int i = 0; i < keyOptions.Length; i++)
        {
            keyOptions[i].LoadDefault();
        }

        Save();
        ApplyLD();
    }

    public void ApplyLD()
    {
        //------ GameplayLD ------
        
        showHints = PlayerPrefs.GetInt("ShowHints_index");
        
        //------ GraphicsLD ------
        
        //Resolution and fullscreen is loaded in the class itself
        resolution = Screen.currentResolution;
        Debug.Log(Screen.width + " x " + Screen.height);

        //QualityLevel
        qualityLevel = PlayerPrefs.GetInt("QualityLevel_index");
        QualitySettings.SetQualityLevel(qualityLevel, true);
        
        
        //------ AudioLD ------
        masterVolume = PlayerPrefs.GetFloat("MasterVolume_value");
    }

    public void ChangeMenu(GameObject menu)
    {
        for (int i = 0; i < settingMenuButtons.Length; i++)
        {
            settingMenuButtons[i].SetActive(false);
        }
        menu.SetActive(true);
    }
}