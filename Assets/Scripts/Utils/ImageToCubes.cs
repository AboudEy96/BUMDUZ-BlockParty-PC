using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ImageToCubes : MonoBehaviour
{
    public Texture2D inputTexture;
    private ColorByName _colorByName = ColorByName.Instance;

    public int width = 45;
    public int height = 48;

    public GameObject[,] cubes;

    public Material[] palette;

    void Start()
    {
        
        if (cubes == null || cubes.Length == 0)
        {
            GenerateGridFromChildren();
        }

        
        ApplyImage();
    }

    public void ApplyImage()
    {
        if (inputTexture == null)
        {
            Debug.LogError("No texture assigned!");
            return;
        }

        Texture2D resized = ResizeTexture(inputTexture, width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = resized.GetPixel(x, y);

                Material closestMat = GetClosestMaterial(pixel);

                Renderer rend = cubes[x, y].GetComponent<Renderer>();
                rend.material = closestMat;
            }
        }
    }

    Material GetClosestMaterial(Color target)
    {
        Material closest = palette[0];
        float minDistance = Mathf.Infinity;

        foreach (Material mat in palette)
        {
            Color c = mat.color;

            float dist =
                (target.r - c.r) * (target.r - c.r) +
                (target.g - c.g) * (target.g - c.g) +
                (target.b - c.b) * (target.b - c.b);

            if (dist < minDistance)
            {
                minDistance = dist;
                closest = mat;
            }
        }

        return closest;
    }

    Texture2D ResizeTexture(Texture2D source, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);

        Graphics.Blit(source, rt);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(width, height, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }

    void GenerateGridFromChildren()
    {
        cubes = new GameObject[width, height];

        GameObject[] cubeList = GetComponentsInChildren<Transform>()
            .Where(t => t != transform)
            .Select(t => t.gameObject)
            .ToArray();

        System.Array.Sort(cubeList, (a, b) =>
        {
            if (Mathf.Abs(a.transform.position.z - b.transform.position.z) > 0.01f)
                return b.transform.position.z.CompareTo(a.transform.position.z);

            return a.transform.position.x.CompareTo(b.transform.position.x);
        });

        int index = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (index < cubeList.Length)
                {
                    cubes[x, y] = cubeList[index];
                    index++;
                }
            }
        }
    }
}