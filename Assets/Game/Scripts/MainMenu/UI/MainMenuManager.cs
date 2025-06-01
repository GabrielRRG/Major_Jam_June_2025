using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button _loadGameButton;
    [SerializeField] private GameObject _warningPopup;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("SceneToLoad"))
        {
            _loadGameButton.interactable = true;
        }
    }

    public void TryNewGame()
    {
        if (PlayerPrefs.HasKey("SceneToLoad"))
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
        PlayerPrefs.DeleteKey("SceneToLoad");
        PlayerPrefs.SetInt("SceneToLoad", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene($"LoadingScene");
    }
    
    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SceneToLoad"))
        {
            Debug.LogWarning("There is no save itself, Load Game is not available yet.");
            return;
        }
        int savedIndex = PlayerPrefs.GetInt("SceneToLoad");
        PlayerPrefs.SetInt("SceneToLoad", savedIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene($"LoadingScene");
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
