using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[Serializable]
public sealed class SettingsDataOldd
{
    public GameplaySettings Gameplay = new();
    public GraphicsSettings Graphics = new();
    public AudioSettings Audio = new();
    public ControlsSettings Controls = new();
}

[Serializable]
public sealed class GameplaySettings
{
    public int InvertX = 0;
    public int InvertY = 0;
}

[Serializable]
public sealed class GraphicsSettings
{
    public int QualityLevel = 1;
    public int ResolutionIndex = 0;
    public int Fullscreen = 1;
}

[Serializable]
public sealed class AudioSettings
{
    public int MasterVolume = 100;
    public int MusicVolume = 100;
    public int EffectsVolume = 100;
}

[Serializable]
public sealed class ControlsSettings
{
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
    public int MouseSensitivity = 30;
}