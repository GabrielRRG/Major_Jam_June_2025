using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private GameObject _warningPopup;
    
    private const string LOADING_KEY = "SceneToLoad";
    private const string PREFS_BACKPACK = "Player_Backpack";
    private const string PREFS_CURRENT_INDEX = "Player_CurrentTool";
    private void Awake()
    {
        if (PlayerPrefs.HasKey(LOADING_KEY))
        {
            _loadGameButton.interactable = true;
        }
    }

    public void TryNewGame()
    {
        if (PlayerPrefs.HasKey(LOADING_KEY))
        {
            _warningPopup.SetActive(true);
        }
        else
        {
            NewGame();
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey(LOADING_KEY);
        PlayerPrefs.DeleteKey(PREFS_BACKPACK);
        PlayerPrefs.DeleteKey(PREFS_CURRENT_INDEX);
        PlayerPrefs.SetInt(LOADING_KEY, 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScene");
    }
    
    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey(LOADING_KEY))
        {
            Debug.LogWarning("There is no save itself, Load Game is not available yet.");
            return;
        }
        int savedIndex = PlayerPrefs.GetInt(LOADING_KEY);
        PlayerPrefs.SetInt(LOADING_KEY, savedIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene("LoadingScene");
    }
    
    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);
    }

    public void CloseMenu(GameObject menu)
    {
        menu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
