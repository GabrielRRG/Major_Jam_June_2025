using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private const string LOADING_KEY = "SceneToLoad";

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        if(Inventory.instance != null) Inventory.instance.SaveBackpack();
        PlayerPrefs.SetInt(LOADING_KEY, PlayerPrefs.GetInt(LOADING_KEY) + 1);
        if (PlayerPrefs.GetInt(LOADING_KEY) >= 5)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        SceneManager.LoadScene("LoadingScene");
    }
}
