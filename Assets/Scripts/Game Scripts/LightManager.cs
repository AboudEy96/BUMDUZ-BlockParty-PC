using System;
using System.Collections;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public GameObject[] lights;
    private Light[] lightComponents;

    void Start()
    {
        // Cache the Light components
        lightComponents = new Light[lights.Length];
        for (int i = 0; i < lights.Length; i++)
        {
            lightComponents[i] = lights[i].GetComponent<Light>();
        }

        // Start fading lights
        foreach (Light light in lightComponents)
        {
            StartCoroutine(FadeLight(light, 4.72f, 9f, 1)); // مثال: fade in من 0 إلى 15 على مدى 2 ثانية
        }
    }

    IEnumerator FadeLight(Light light, float fadeStart, float fadeEnd, float fadeTime)
    {
        float t = 0.0f; 

        while (true)
        {
            // in
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                light.intensity = Mathf.Lerp(fadeStart, fadeEnd, t / fadeTime);
                yield return null; 
            }
            // end = start , start = end
            float temp = fadeStart;
            fadeStart = fadeEnd;
            fadeEnd = temp;
            t = 0.0f;
        }
    }
}