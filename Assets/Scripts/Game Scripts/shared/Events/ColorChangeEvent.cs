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
        int cubeLayer = LayerMask.NameToLayer("Cube");

        foreach (Transform cube in map)
        {
            if (cube.gameObject.layer != cubeLayer) continue;

            Renderer r = cube.GetComponent<Renderer>();
            if (r == null) continue;

            string materialName = r.material.name
                .Replace("(Instance)", "")
                .Replace("(Clone)", "")
                .Trim();

            int spaceIndex = materialName.IndexOf(' ');
            if (spaceIndex > 0)
                materialName = materialName.Substring(0, spaceIndex);

            if (string.IsNullOrEmpty(materialName) || materialName.Contains("Default"))
                materialName = "White";
            try
            {
                cube.tag = materialName;
            }
            catch
            {
                cube.tag = "Untagged";
                Debug.LogWarning($"Tag not found: {materialName} on {cube.name}");
            }
        }
    }

}