using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowOption : OptionBase
{
    public int defaultIndex;
    public int currentIndex;
    public List<String> options = new List<string>();
    [SerializeField] private TMP_Text _optionText;
    public override void Load()
    {
        Debug.Log(currentIndex);
        currentIndex = PlayerPrefs.GetInt(optionName + "_index",defaultIndex);
        _optionText.text = options[currentIndex];
        base.Load();
    }

    public override void LoadDefault()
    {
        currentIndex = defaultIndex;
        _optionText.text = options[currentIndex];
        base.LoadDefault();
    }

    public void NextOption()
    {
        currentIndex = (currentIndex + 1) % options.Count;
        _optionText.text = options[currentIndex];
    }

    public void PreviousOption()
    {
        currentIndex = (currentIndex - 1 + options.Count) % options.Count;
        _optionText.text = options[currentIndex];
    }
}