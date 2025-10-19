using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectColor : MonoBehaviour
{

    public Material skinColor;

    [SerializeField]
    private Transform BUTTON_PARENT;
    [SerializeField] private GameObject Character;
    [SerializeField] private List<Material> _materialsSkins;
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
        SkinnedMeshRenderer sms = Character.GetComponentInChildren<SkinnedMeshRenderer>(); 
        foreach (var mat in _materialsSkins)
        {
            if (mat.name == PlayerPrefs.GetString("Skin"))
            {
                sms.GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
                break;
            }
        }
        Debug.Log(PlayerPrefs.GetString("Skin"));
    }
}