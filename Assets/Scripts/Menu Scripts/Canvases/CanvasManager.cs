using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static event Action<string> OnCloseClick;

    [Header("Main Menu Canvases")] [SerializeField]
    private GameObject[] Canvases;

    public void OnEnable()
    {
        OnCloseClick += ShowCanvas;
    }

    public void OnDisable()
    {
        OnCloseClick -= ShowCanvas;
    }

    public void ShowCanvas(string canvasName = "Canavs [ UI - MAIN ]")
    {
        canvasName ??= "Canavs [ UI - MAIN ]";

        foreach (GameObject canvas in Canvases)
        {
            canvas.SetActive(false);
            if (canvas.name == canvasName)
            {
                canvas.SetActive(true);
            }
        }
    }
    public static void CloseClick(string canvasName = null)
    {
        OnCloseClick?.Invoke(canvasName);
    }
}
