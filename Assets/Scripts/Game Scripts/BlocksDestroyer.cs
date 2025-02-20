using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BlocksDestroyer : MonoBehaviour
{

    public MapChanger _MapChanger;
    public GameObject map;
    public static string chosenTag;
    
    private List<Transform> allCubes = new List<Transform>();
    public static int score = 0;
    public Transform theImages;
    private void Start()
    {
        Cursor.visible = false;
        map = _MapChanger.maps[_MapChanger.getCurrentMapIndex()];
        Invoke("SelectRandomColor", 2f);
    }
    private void ShowSelectedColor(Transform image, string tag) 
    {
        foreach (Transform color in image)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(color.CompareTag(tag));
        }
    }
    private void SelectRandomColor()
    {
        foreach (Transform child in map.transform) {
           if (child.gameObject.layer == LayerMask.NameToLayer("Cube"))
           { 
               allCubes.Add(child);
           }
        }
        
        HashSet<string> tags = new HashSet<string>();
        foreach (Transform cube in allCubes)
        {
            tags.Add(cube.tag);
        }

        string[] tagArray = new string[tags.Count];
        tags.CopyTo(tagArray);

        // اختيار تاق عشوائي
         chosenTag = tagArray[Random.Range(0, tagArray.Length)];
         ShowSelectedColor(theImages, chosenTag);
         Debug.Log(chosenTag + " has been chosen OTHER COLORS will be destroyed after 5 seconds!");
         Invoke("DestroyCubes", 5f);
    }
    
       private void DestroyCubes()
    {
        // تدمير المكعبات التي تحتوي على layer cube وليس لديها التاق الذي تم اختياره
        foreach (Transform cube in allCubes)
        {
            if (cube.tag != chosenTag)
            {
                cube.gameObject.SetActive(false);
            }
        }
        Debug.Log("DESTROYED OTHER COLORS OF" + chosenTag);
        allCubes.Clear();

        Invoke("ActiveNextMap", 5f);
    }

private void ActiveNextMap()
{
        _MapChanger.runNextMap();
        map = _MapChanger.maps[_MapChanger.getCurrentMapIndex()];
        foreach (Transform allCube in allCubes)
        {
            allCube.gameObject.SetActive(true);
        }
        {
            
        }
        foreach (Transform mapCubes in map.transform)
        {
            mapCubes.gameObject.SetActive(true);
        }
        Debug.Log(_MapChanger.getMapName());
        score++;
        Debug.Log("Your score is " + score);
        Invoke("SelectRandomColor", 3f);
    }
}
