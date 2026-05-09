using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public static Title Instance;

    private TextMeshProUGUI txt;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            GameObject canvasObj = new GameObject("TitleCanvas");
            canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();

            DontDestroyOnLoad(canvasObj);

            GameObject textObj = new GameObject("TitleText");
            textObj.transform.SetParent(canvasObj.transform, false);

            txt = textObj.AddComponent<TextMeshProUGUI>();
            txt.alignment = TextAlignmentOptions.Center;
            txt.fontSize = 36;
            txt.rectTransform.anchoredPosition = Vector2.zero;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetTitle(string newTitle, bool clear)
    {
        txt.text = newTitle;
        if (clear) StartCoroutine(ClearTitle());
    }

    public void SetTitle(string newTitle)
    {
        txt.text = newTitle;
    }

    IEnumerator ClearTitle()
    {
        yield return new WaitForSeconds(4);
        txt.text = "";
    }
}