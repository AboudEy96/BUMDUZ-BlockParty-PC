using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class CubesOptimization : MonoBehaviour
{
 public GameObject MapParent;

 public List<string> ExistColors = new List<string>();
public List<GameObject> ObjectColors = new List<GameObject>();



void Update()
{
 if (Input.GetKeyDown(KeyCode.E))
 {
  Encapsulation();
 }
}

// add ExistColors as gameobjects, after adding the existcolors add the colors to thier parents

 void Encapsulation()
 {
  var map = MapParent.transform;
  ColorChangeEvent.SetUpColors(map);
  List<Transform> cubes = new List<Transform>();
  for (int i = 0; i < map.childCount; i++)
  {
   cubes.Add(map.GetChild(i));
  }

  for (int i = 0; i < cubes.Count; i++)
  {
   if (!ExistColors.Contains(cubes[i].tag))
   {
    // add name to exist colors
    ExistColors.Add(cubes[i].tag);
    // create object and add to objects list ( so we can add the cube from another loop to the object as childern )
    GameObject color = new GameObject(cubes[i].tag);
    color.transform.SetParent(map);
    color.tag = cubes[i].tag;
    ObjectColors.Add(color);
   }
  }

  // add color to object parent
  for (int i = 0; i < cubes.Count; i++)
  {
   for (int j = 0; j < ObjectColors.Count; j++)
   {
    if (cubes[i].tag == ObjectColors[j].tag)
    {
     Debug.Log(cubes[i].tag + " -> " + ObjectColors[j].name);
     cubes[i].SetParent(ObjectColors[j].transform);
     break;
    }
   }
  }
 }
 
}
