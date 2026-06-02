using System.Collections.Generic;
using UnityEngine;

public class ColorByName : MonoBehaviour
{
    public static ColorByName Instance; 
    
    public List<Material> MaterialColors;

    [System.Serializable]
    public class ColorData
    {
        public string Windows;
        public string Mobile;
    }

public Dictionary<string, ColorData> Colors = new Dictionary<string, ColorData>()
{
    { "Aqua", new ColorData() { Windows = "#28b1c9", Mobile = "#219CB2" } },
    { "Black", new ColorData() { Windows = "#2b2931", Mobile = "#201E24" } },
    { "Blue", new ColorData() { Windows = "#445AD7", Mobile = "#526CF7" } },
    { "DarkBlue", new ColorData() { Windows = "#4837AC", Mobile = "#4D3BB5" } },
    { "Green", new ColorData() { Windows = "#B7C22B", Mobile = "#BCC62E" } }, // 
    { "Gray", new ColorData() { Windows = "#9789c2", Mobile = "#B1A1E2" } }, // B1A1E1
    { "LightBiege", new ColorData() { Windows = "#e2c694", Mobile = "#EBCE9B" } },
    { "LightGray", new ColorData() { Windows = "#989db2", Mobile = "#C4CBE6" } },
    { "LightPurple", new ColorData() { Windows = "#e550fa", Mobile = "#ED53FF" } },
    { "Lime", new ColorData() { Windows = "#CEE53B", Mobile = "#D1E93D" } },
    { "Magenta", new ColorData() { Windows = "#e968fc", Mobile = "#F26DFF" } },
    { "Navy", new ColorData() { Windows = "#3b3573", Mobile = "#40387B" } },
    { "Orange", new ColorData() { Windows = "#df910e", Mobile = "#F09D0C" } },
    { "Pink", new ColorData() { Windows = "#f1a3f7", Mobile = "#F3A5FA" } },
    { "Purple", new ColorData() { Windows = "#8f49ec", Mobile = "#944CF1" } },
    { "Red", new ColorData() { Windows = "#eb510d", Mobile = "#F2560D" } },
    { "White", new ColorData() { Windows = "#EBEDFD", Mobile = "#E3E5FA" } },
    { "Yellow", new ColorData() { Windows = "#eae40f", Mobile = "#EBE50D" } },
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

    public string GetColorByName(string name, string type)
    {
        switch (type)
        {
            case "material":

                foreach (var cr in MaterialColors)
                {
                    if (name.Equals(cr.name))
                        return "#" + ColorUtility.ToHtmlStringRGB(cr.color);
                }
                break;
            case "pc":
                return Colors[name].Windows;

            case "mobile":

                return Colors[name].Mobile;
        }
        return null;
    }
}