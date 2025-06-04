#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ResArrowOption : ArrowOption
{
    private void Start()
    {
        SettingsManager.instance.SaveAll += Load;
    }

    private void OnDestroy()
    {
        SettingsManager.instance.SaveAll -= Load;
    }

    private void Reset()
    {
        List<string> allResStrings = Screen.resolutions
            .Select(r => $"{r.width}x{r.height}")
            .ToList();

        foreach (string custom in options)
        {
            if (!allResStrings.Contains(custom))
                allResStrings.Add(custom);
        }

        allResStrings = allResStrings
            .Select(s => new { str = s, area = ParseArea(s) })
            .OrderBy(x => x.area)
            .Select(x => x.str)
            .ToList();

        options.Clear();
        options.AddRange(allResStrings);

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    private int ParseArea(string resolution)
    {
        var parts = resolution.Split('x');
        if (parts.Length != 2) return 0;
        if (int.TryParse(parts[0], out int w) && int.TryParse(parts[1], out int h))
            return w * h;
        return 0;
    }

    public override void Load()
    {
        base.Load();

        if (currentIndex < 0 || currentIndex >= options.Count)
            return;

        string[] parts = options[currentIndex].Split('x');
        if (parts.Length == 2 &&
            int.TryParse(parts[0], out int width) &&
            int.TryParse(parts[1], out int height))
        {
            Screen.SetResolution(width, height, true);
            Debug.Log($"Permission has been granted: {width}x{height}");
        }
        else
        {
            Debug.LogWarning($"Unable to unparser the resolution: {options[currentIndex]}");
        }
    }
}