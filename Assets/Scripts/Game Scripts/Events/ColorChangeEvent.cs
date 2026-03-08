using UnityEngine;

public class ColorChangeEvent : MonoBehaviour
{
    
 //   [Header("The Map Object")]private static Transform MAP;

    private string[] colors =
    {
        "Aqua", "Black", "Blue", "DarkBlue", "Gray",
        "InvisiblePurple", "LightBeige",
        "LightPurple", "Navy", "Orange",
        "Pink", "Purple", "Red", "White"
    };

    public static void SetUpColors(Transform map)
    {
        foreach (Transform cube in map)
        {
            if (cube.gameObject.layer == LayerMask.NameToLayer("Cube"))
            {
                Material material = cube.GetComponent<Renderer>().material;
                string materialName = material.name.Replace("(Instance)", "").Trim();
                if (materialName.Contains("Default"))
                {
                    materialName = "White";
                }
                cube.tag = materialName; 
                    
            }
        }
    }

}