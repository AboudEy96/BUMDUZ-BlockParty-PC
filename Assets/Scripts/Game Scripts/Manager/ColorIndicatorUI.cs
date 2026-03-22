using UnityEngine;

public class ColorIndicatorUI : MonoBehaviour
{
    [SerializeField] private Transform imagesContainer;

    public void Show(string tag)
    {
        foreach (Transform color in imagesContainer)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(color.CompareTag(tag));
        }
    }

    public void Hide()
    {
        foreach (Transform color in imagesContainer)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(false);
        }
    }
}