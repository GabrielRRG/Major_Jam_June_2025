using System;

public interface ISettingOptionOld
{
    string Key { get; }
    bool IsDefault();
    void ResetToDefault();
    string GetValueAsString();
    void SetValueFromString(string s);
    event Action OnChanged;
}