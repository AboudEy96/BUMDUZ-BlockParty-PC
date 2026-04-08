using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorByName : MonoBehaviour
{
    public static ColorByName Instance; 
    
    public List<Material> MaterialColors;
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


public string GetColorByName(string name)
    {
        foreach (var cr in MaterialColors)
        {
            if (name.Equals(cr.name))
            {
                Color color = cr.color;
                return "#" + ColorUtility.ToHtmlStringRGB(cr.color);
            }
        }
        return null;
    }
}