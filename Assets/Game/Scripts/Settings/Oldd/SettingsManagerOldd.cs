using UnityEngine;

public class SettingsManagerOldd : MonoBehaviour
{
    [Header("Gameplay")]
    [SerializeField] private ArrowOptionOldd _invertX;

    public void ApplySettings()
    {
        _invertX.SetOptionID(SettingsSaveLoadOldd.instance.SettingsDataOldd.Gameplay.InvertX);
    }
}