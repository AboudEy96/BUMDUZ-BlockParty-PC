using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class ButtonClickMangments : MonoBehaviour,IButtonClickMangment
{
     public List<Sprite> images = new List<Sprite>();
     public List<GameObject> buttons = new List<GameObject>();
     public Transform theImage;
     private string currentMode;
     [Header("The Menu of choose object")] public GameObject chooseModeObject;
     public void Awake()
     {
         ActiveButtons();
     }
    public void Play(GameObject button)
    {
        string buttonName = button.transform.name;
        switch (buttonName)
        {
            case "Singleplayer":
                PhotonNetwork.Disconnect();
                PhotonNetwork.OfflineMode = true;

                PhotonNetwork.CreateRoom("OfflineRoom");
                SceneManager.LoadScene("Game");
                break;
            default:
                chooseModeObject.SetActive(true);
                break;
        }
        
    }
        
    public void Other(GameObject button)
    {
        foreach (Sprite image in images)
        {
            if (button.name == image.name)
            {
                Image img = theImage.GetComponent<Image>();
                img.sprite = image;
                ActiveButtons();
            }
        }
    }

    public string GetCurrentMode()
    {
        Image img = theImage.GetComponent<Image>();
        currentMode = img.sprite.name;
        return currentMode;
    }

    public void ActiveButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(button.name.Contains(GetCurrentMode()));
        }   
    }

}
