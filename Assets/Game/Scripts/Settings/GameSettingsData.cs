using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "Settings/Game Settings")]
public sealed class GameSettingsData : ScriptableObject
{
    //This is a temporary implementation
    
    [Header("Gameplay Settings")]
    public int language;
    public bool tutorialHintsEnabled;
    public bool vibrateEnabled;
    public bool invertMouseX;
    public bool invertMouseY;
    public float mouseSensitivity;
    public bool HUDEnabled;

    [Header("Graphics Settings")]
    public bool vsyncEnabled;
    public bool fullscreen;
    public float graphicsQuality;
    public int textureQuality;
    public bool bloom;
    public bool ambientOcclusion;
    public int antiAliasing;
    public bool anisotropicFiltering;

    [Header("Audio Settings")]
    public string audioDevice;
    public float masterVolume;
    public float musicVolume;
    public float voiceVolume;
    public float sfxVolume;
    public int muteType;
}