using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class ColorIndicatorUI : MonoBehaviour
{
    [SerializeField] private Transform imagesContainer;
    [SerializeField] private GameObject mobileColors;
    [SerializeField] private GameObject cubeUI;
    [SerializeField] private TextMeshPro selectedColorText; 
    
    private ColorByName _colorByName = ColorByName.Instance;
    
    public void Show(string tag)
    {
        
        #if UNITY_ANDROID || UNITY_IOS
        imagesContainer.gameObject.SetActive(false);
        mobileColors.SetActive(true);
        MeshRenderer cubeColor = cubeUI.GetComponent<MeshRenderer>();
        cubeColor.material = _colorByName.MaterialColors.FirstOrDefault(m => m.name.Equals(tag, System.StringComparison.OrdinalIgnoreCase));
        selectedColorText.text = tag;
        string hex = ColorByName.Instance.GetColorByName(selectedColorText.text);
        
        if (!string.IsNullOrEmpty(hex))
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(hex, out color))
            {
                selectedColorText.color = color;
            }
        }
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