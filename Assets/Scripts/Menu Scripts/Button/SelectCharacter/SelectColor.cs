using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectColor : MonoBehaviour
{

    public Material skinColor;

    [SerializeField]
    private Transform BUTTON_PARENT;
    private void Start()
    {
        Button[] buttons = BUTTON_PARENT.GetComponentsInChildren<Button>();

        foreach (Button bt in buttons)
        {
            bt.onClick.AddListener((() => ButtonClick(bt)));
        }
    }

    public void ButtonClick(Button button)
    {
        PlayerPrefs.SetString("Skin", button.name);
        Debug.Log(PlayerPrefs.GetString("Skin"));
    }
}