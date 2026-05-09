using System.Collections.Generic;
using UnityEngine;

public class ColorByName : MonoBehaviour
{
    public static ColorByName Instance; 
    
    public List<Material> MaterialColors;
    //                tag   - hex
    public Dictionary<string, string> UICubeColor = new Dictionary<string, string>
    {
        { "Aqua", "#28b1c9"},
        { "Black", "#2b2931"},
        { "Blue", "#445AD7"},
        { "DarkBlue", "#4837AC"},
        { "Green", "#B7C22B"},
        { "Gray", "#9789c2"},
        { "LightBiege", "#e2c694"},
        { "LightGray", "#989db2"},
        { "LightPurple", "#e550fa"},
        { "Lime", "#CEE53B"},
        { "Magenta", "#e968fc"},
        { "Navy", "#3b3573"},
        { "Orange", "#df910e"}, 
        { "Pink", "#f1a3f7"},
        { "Purple", "#8f49ec"},
        { "Red", "#eb510d"},
        { "White", "#EBEDFD"},
        { "Yellow", "#eae40f"},
    //    { ""}
    };
    //private string[] TextColors = { "red","blue", "green", "lime" };
// RED -> Material Red

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


public string GetColorByName(string name, string type) // material, uihex
    {
        switch (type)
        {
            case "material":
                foreach (var cr in MaterialColors)
                {
                    if (name.Equals(cr.name)) return "#" + ColorUtility.ToHtmlStringRGB(cr.color);
                }
                break;
            case "tag":
                try
                {
                    return UICubeColor[name];
                }
                catch (KeyNotFoundException e)
                {
                    Debug.Log(e.Message);
                }

                break;
        }
  
        return null;
    }
}