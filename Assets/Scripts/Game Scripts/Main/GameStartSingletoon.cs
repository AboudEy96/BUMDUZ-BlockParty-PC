using System;
using UnityEngine;

public class GameStartSingletoon : MonoBehaviour
{
    public static GameStartSingletoon instance;

    public bool isGameStarted { get; private set; } = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    public static GameStartSingletoon GetInstance()
    {
        return instance;
    }

    public void GameStart()
    {
        isGameStarted = true;
    }

    public void GameEnd()
    {
        isGameStarted = false;
    }
}