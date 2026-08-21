using System;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static event Action<string> OnCloseClick;
    public static CanvasManager instance;
    [Header("Main Menu Canvases")] [SerializeField]
    private GameObject[] Canvases;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

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
    public static void CloseClick(string canvasName = null) // which canvas we should show after cloese all canvases ( UI - MAIN ) by default.
    {
        OnCloseClick?.Invoke(canvasName);
    }

    public GameObject[] GetCanvases()
    {
        return Canvases;
    }
}
