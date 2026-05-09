using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class ColorIndicatorUI : MonoBehaviour
{
    [SerializeField] private Transform imagesContainer;
    [SerializeField] private GameObject mobileColors;
    [SerializeField] private  GameObject cubeLabelUI;
    [SerializeField] private TextMeshPro selectedColorText; 
    
    private ColorByName _colorByName = ColorByName.Instance;
    
public void Show(string tag)
{
    imagesContainer.gameObject.SetActive(false);
    mobileColors.SetActive(true);
    SpriteRenderer cubeColor = cubeLabelUI.GetComponent<SpriteRenderer>();
    if (_colorByName.UICubeColor.TryGetValue(tag, out string hex))
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            cubeColor.color = color; 
            selectedColorText.color = color;
        }
    }

    selectedColorText.text = tag;
/*#else
        foreach (Transform color in imagesContainer)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(color.CompareTag(tag));
        } 

    #endif*/
}


    public void Hide()
    {
#if UNITY_ANDROID || UNITY_IOS
        mobileColors.SetActive(false);
     #else
        mobileColors.SetActive(false);
      /*  foreach (Transform color in imagesContainer)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(false);
        }*/
    #endif
}
    
}