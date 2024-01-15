using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ScreenManager : MonoSingleton<ScreenManager>
{
    private List<string> sceneNameList = new List<string>();

    [Header("FadeScreen")]
    [SerializeField] private float fadeSpeed;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float fadeDelayTime = 1f;
    private Image fadeScreenImage;

    private void Awake()
    {
        fadeScreenImage = GetComponentInChildren<Image>();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            sceneNameList.Add(sceneName);
        }
    }

    private bool IsSceneExist(string sceneName)
    {
        bool isExist = false;
        foreach (string name in sceneNameList)
        {
            if (name == sceneName)
            {
                isExist = true;
                break;
            }
        }
        return isExist;
    }

    public void ChangeScreen(string sceneName)
    {
        if (IsSceneExist(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"{sceneName} is not exist");
        }
    }

    public void ChangeCursorMode(string sceneName)
    {
        if (sceneName == "Intro")
        {
            UIManager.Instance.ShowCursor();
        }
        else
        {
            UIManager.Instance.LockCursor();
        }
    }

    public void ChangeScreenWithFade(string sceneName)
    {
        StartCoroutine(FadeScreenCor(sceneName));
    }

    private IEnumerator FadeScreenCor(string sceneName)
    {
        fadeScreenImage.enabled = true;

        Color alpha = fadeScreenImage.color;
        float elapsedTime = 0f;
        float value = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            value = elapsedTime / fadeDuration;

            alpha.a = Mathf.Lerp(0, 1, value);
            fadeScreenImage.color = alpha;

            yield return null;
        }

        ChangeScreen(sceneName);
        ChangeCursorMode(sceneName);
        yield return new WaitForSeconds(fadeDelayTime);

        alpha = fadeScreenImage.color;
        elapsedTime = 0f;
        value = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            value = elapsedTime / fadeDuration;

            alpha.a = Mathf.Lerp(1, 0, value);
            fadeScreenImage.color = alpha;

            yield return null;
        }

        fadeScreenImage.enabled = false;
    }
}
