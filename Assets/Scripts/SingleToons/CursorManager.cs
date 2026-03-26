using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D curosImg;
    [SerializeField] private Vector2 clickPos = Vector2.zero;

    private static CursorManager Instance;    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.SetCursor(curosImg, clickPos, CursorMode.Auto);
    }
}
