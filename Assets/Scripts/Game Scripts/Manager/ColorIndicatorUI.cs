using TMPro;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class ColorIndicatorUI : MonoBehaviour
{
    [SerializeField] private Transform imagesContainer;
    [SerializeField] private GameObject mobileColors;
    [SerializeField] private GameObject cubeLabelUI;
    [SerializeField] private TextMeshPro selectedColorText;

    private ColorByName _colorByName;

    private string CurrentPlatform;

    private void Awake()
    {
        _colorByName = ColorByName.Instance;
    }

    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        CurrentPlatform = "mobile";
#else
        CurrentPlatform = "windows";
#endif
    }

    public void Show(string tag)
    {
        imagesContainer.gameObject.SetActive(false);
        mobileColors.SetActive(true);

        SpriteRenderer cubeColor = cubeLabelUI.GetComponent<SpriteRenderer>();

        if (_colorByName.Colors.TryGetValue(tag, out var colorData))
        {
            string hex =
                CurrentPlatform == "mobile"
                    ? colorData.Mobile
                    : colorData.Windows;

            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                cubeColor.color = color;
                selectedColorText.color = color;
            }
        }

        selectedColorText.text = tag;
    }

    public void Hide()
    {
        mobileColors.SetActive(false);
    }
}