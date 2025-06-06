using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    private const string LOADING_KEY = "SceneToLoad";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) NextLevel();
    }

    public void NextLevel()
    {
        for (int i = 0; i < Inventory.instance.backpack.Length; i++)
        {
            switch (Inventory.instance.backpack[i].GetComponent<Gun>().gunData.gunName)
            {
                case "Pistol":
                {
                    PlayerPrefs.SetString("Slot" + i, "Pistol");
                }
                    break;
                case "Shotgun":
                {
                    PlayerPrefs.SetString("Slot" + i, "Shotgun");
                }
                    break;
                case "AsaultRifle":
                {
                    PlayerPrefs.SetString("Slot" + i, "AsaultRifle");
                }
                    break;
                default:
                {
                    PlayerPrefs.DeleteKey("Slot" + i);
                }
                    break;
            }
        }
        PlayerPrefs.SetInt(LOADING_KEY, PlayerPrefs.GetInt(LOADING_KEY) + 1);
        SceneManager.LoadScene("LoadingScene");
    }
}
