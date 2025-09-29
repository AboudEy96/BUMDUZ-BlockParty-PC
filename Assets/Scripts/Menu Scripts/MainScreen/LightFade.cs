using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class LightFade : MonoBehaviour
{
    [SerializeField] private Image theLight;
    
    void Start() 
    {
        theLight = this.gameObject.GetComponent<Image>();
        StartCoroutine(LightFadeIn());
    }

    IEnumerator LightFadeIn()
    {

        while (true)
        {
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                Color c = theLight.color;
                c.a = Mathf.Lerp(0.3f, 1f, t);
                theLight.color = c;
                yield return null;
            }

            Color c1 = theLight.color;
            c1.a = 1f;
            theLight.color = c1;
            yield return new WaitForSeconds(0.5f);

            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                Color c = theLight.color;
                c.a = Mathf.Lerp(1f, 0.3f, t);
                theLight.color = c;
                yield return null;
            }

            Color c2 = theLight.color;
            c2.a = 0f;
            theLight.color = c2;
        } 
    }
}
