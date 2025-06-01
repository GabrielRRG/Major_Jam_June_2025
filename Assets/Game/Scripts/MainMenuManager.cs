using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        if (PlayerPrefs.HasKey("SavedLevelIndex"))
            PlayerPrefs.DeleteKey("SavedLevelIndex");
        
        PlayerPrefs.SetString("SceneToLoad", "Level 1");
        PlayerPrefs.Save();
        SceneManager.LoadScene($"LoadingScene");
    }
    
    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SavedLevelIndex"))
        {
            Debug.LogWarning("There is no save itself, Load Game is not available yet.");
            return;
        }
        int savedIndex = PlayerPrefs.GetInt("SavedLevelIndex");
        string savedSceneName = "Level " + savedIndex;
        PlayerPrefs.SetString("SceneToLoad", savedSceneName);
        PlayerPrefs.Save();
        SceneManager.LoadScene($"LoadingScene");
    }
}
