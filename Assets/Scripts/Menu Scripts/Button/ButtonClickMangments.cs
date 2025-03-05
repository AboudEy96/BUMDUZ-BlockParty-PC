using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ButtonClickMangments : MonoBehaviour,IButtonClickMangment
{
     public List<Texture> Images = new List<Texture>();
     public RawImage displayImage; 


    public void Play(GameObject button)
    {
        string buttonScene = button.transform.name;
        SceneManager.LoadScene(buttonScene);

    }

    public void Test(GameObject button)
    {
        foreach (Texture image in Images)
        {
            if (button.name == image.name)
            {
                image.GameObject().SetActive(true);
            } 
        }
    }


    public void ChangeMode()
    {
            foreach (Texture image in Images)
            {
                if (image.name == "ChangeMode")
                {
                    Debug.Log("ChangeMode");
                    image.GameObject().SetActive(true);
                }
                else
                {
                    image.GameObject().SetActive(false);
                }
            }
        
    } 

    public void Profile()
    {
        
    }

    public void Character()
    {
        
    }

    public void Map()
    {
        
    }

    public void Shop()
    {
        
    }

    public void Settings()
    {
        
    }

}
