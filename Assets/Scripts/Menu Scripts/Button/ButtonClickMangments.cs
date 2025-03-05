using System.Collections.Generic;
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

     public void Awake()
     {
         ActiveButtons();
     }
    public void Play(GameObject button)
    {
        string buttonScene = button.transform.name;
        SceneManager.LoadScene(buttonScene);

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
        return img.sprite.name;
    }

    public void ActiveButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(button.name.Contains(GetCurrentMode()));
        }   
    }

}
