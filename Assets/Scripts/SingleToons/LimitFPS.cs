using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    void Awake()
    {
   //     QualitySettings.vSyncCount = 0;
//        Application.targetFrameRate = 60;

        DontDestroyOnLoad(gameObject);
    }
}
