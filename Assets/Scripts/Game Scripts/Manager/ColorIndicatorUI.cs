using System.Linq;
using UnityEngine;

public class ColorIndicatorUI : MonoBehaviour
{
    [SerializeField] private Transform imagesContainer;
    [SerializeField] private GameObject mobileColors;
    [SerializeField] private GameObject cubeUI;
    [SerializeField] private Material[] cubeColors;
    public void Show(string tag)
    {
        
        #if UNITY_ANDROID || UNITY_IOS
        imagesContainer.gameObject.SetActive(false);
        mobileColors.SetActive(true);
        MeshRenderer cubeColor = cubeUI.GetComponent<MeshRenderer>();
        cubeColor.material = cubeColors.FirstOrDefault(m => m.name.Contains(tag));
     #else
        foreach (Transform color in imagesContainer)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(color.CompareTag(tag));
        } 

       #endif
    }


    public void Hide()
    {
#if UNITY_ANDROID || UNITY_IOS
        mobileColors.SetActive(false);
     #else
        foreach (Transform color in imagesContainer)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(false);
        }
    #endif
}
    
}