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
   private SkinnedMeshRenderer sms;

   

   private void Start()
    {
        sms =  Character.GetComponentInChildren<SkinnedMeshRenderer>();
        SyncCharacterColor();
        Button[] buttons = BUTTON_PARENT.GetComponentsInChildren<Button>();

        foreach (Button bt in buttons)
        {
            bt.onClick.AddListener((() => ButtonClick(bt)));
        }
    }

    public void ButtonClick(Button button)
    {
        PlayerPrefs.SetString("Skin", button.name);
        SyncCharacterColor();
        Debug.Log(PlayerPrefs.GetString("Skin"));
    }

    public void SyncCharacterColor()
    {
        foreach (var mat in SyncPlayerMaterial.instance._skinMaterials)
        {
            if (mat.name == PlayerPrefs.GetString("Skin"))
            {
                sms.GetComponentInChildren<SkinnedMeshRenderer>().material = mat;
                break;
            }
        }

    }
}