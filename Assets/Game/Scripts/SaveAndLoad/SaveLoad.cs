using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    private const string LOADING_KEY = "SceneToLoad";

    public void NextLevel()
    {
        if(Inventory.instance != null) Inventory.instance.SaveBackpack();
        PlayerPrefs.SetInt(LOADING_KEY, PlayerPrefs.GetInt(LOADING_KEY) + 1);
        SceneManager.LoadScene("LoadingScene");
    }
}
