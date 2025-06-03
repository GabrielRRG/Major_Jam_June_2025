using UnityEngine;
using UnityEngine.UI;

public sealed class TabManager : MonoBehaviour
{
    [SerializeField] private SettingsManager _settingsManager;
    [SerializeField] private Button[] _tabButtons;
    [SerializeField] private GameObject[] _tabPanels;

    private int _currentIndex = 0;

    private void Awake()
    {
        if (_tabButtons == null || _tabButtons.Length == 0)
            return;

        for (int i = 0; i < _tabButtons.Length; i++)
        {
            int index = i;
            _tabButtons[i].onClick.AddListener(() => ShowTab(index));
        }
    }

    private void OnEnable()
    {
        ShowTab(0);
    }

    public void ShowTab(int index)
    {
        Debug.Log($"Showing tab: {index}");
        if (_tabButtons == null || index < 0 || index >= _tabButtons.Length)
            return;

        _currentIndex = index;

        for (int i = 0; i < _tabButtons.Length; i++)
        {
            _tabPanels[i].SetActive(i == _currentIndex);
        }

        if (_settingsManager == null) return;
        _settingsManager.CheckResetButton();
        _settingsManager.CheckSaveButton();
    }
}