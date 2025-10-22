using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroTimeLine : MonoBehaviour
{

    public PlayableDirector director;
    
    void Start()
    {
    }

    private void Update()
    {
        if (director.state != PlayState.Playing)
        {
            director.Pause(); // cance repeating
            SceneManager.LoadScene("MainScene");
            
        }
    }
}