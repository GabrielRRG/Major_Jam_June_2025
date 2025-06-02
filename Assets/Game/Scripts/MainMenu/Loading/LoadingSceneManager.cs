using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public sealed class LoadingSceneManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressText;
    
    private const string LOADING_KEY = "SceneToLoad";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(LOADING_KEY))
        {
            Debug.LogError("There is no SceneToLoad key in PlayerPrefs. Nothing to load!");
            return;
        }
        Debug.Log("Loading scene " + PlayerPrefs.GetInt(LOADING_KEY));
        string targetSceneName = "Level " + PlayerPrefs.GetInt(LOADING_KEY);
        // If you wish, you can delete this key immediately so that it does not interfere in the future:
        // PlayerPrefs.DeleteKey(kSceneToLoadKey);
        // PlayerPrefs.Save();
        StartCoroutine(LoadSceneAsync(targetSceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = $"Loading: {(int)(progress * 100f)}%";
            
            if (op.progress >= 0.9f)
            {
                if (progressBar != null)
                    progressBar.value = 1f;
                if (progressText != null)
                    progressText.text = "Loading: 100%";
                yield return new WaitForSeconds(0.2f);
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
