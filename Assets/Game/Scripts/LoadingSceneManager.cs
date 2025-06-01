using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private Text progressText;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("SceneToLoad"))
        {
            Debug.LogError("There is no SceneToLoad key in PlayerPrefs. Nothing to load!");
            return;
        }

        string targetSceneName = PlayerPrefs.GetString("SceneToLoad");
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
