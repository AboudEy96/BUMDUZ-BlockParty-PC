using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarMoving : MonoBehaviour
{
    public GameObject LoaderUI;
    public Slider progressSlider;

    public void LoadScene(int index)
    {
        StartCoroutine(LoadScene_Coroutine(index));
    }

    public IEnumerator LoadScene_Coroutine(int index)
    {
        progressSlider.value = 0;
        LoaderUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Lobby");
        asyncOperation.allowSceneActivation = false;

        float progress = 0;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress < 0.9f)
            {
                progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
                progressSlider.value = progress;
            }
            else
            {
                progress = Mathf.MoveTowards(progress, 1f, Time.deltaTime);
                progressSlider.value = progress;

                if (progress >= 1f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}