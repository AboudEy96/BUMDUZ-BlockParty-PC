using System.Collections;
using TMPro;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class ColorIndicatorUI : MonoBehaviour
{
    [SerializeField] private Transform imagesContainer;
    [SerializeField] private GameObject mobileColors;
    [SerializeField] private GameObject cubeLabelUI;
    [SerializeField] private TextMeshPro selectedColorText;
    [SerializeField] private GameObject cameraCube;
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
        MeshRenderer cameraCubeColor = cameraCube.GetComponent<MeshRenderer>();
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
                cameraCubeColor.material.SetColor("_Color", color);
            }
        }

        selectedColorText.text = tag;
        StartCoroutine(RunCubeAnimation());
    }

    IEnumerator RunCubeAnimation()
    {
        cameraCube.SetActive(true);
        Animator animator = cameraCube.GetComponent<Animator>();
        animator.Play("CubeAnim", -1, 0f);
        yield return null; 
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.IsName("CubeAnim") && stateInfo.normalizedTime < 1.0f)
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null; 
        }

        cameraCube.SetActive(false);
    }
    public void Hide()
    {
        mobileColors.SetActive(false);
    }
}