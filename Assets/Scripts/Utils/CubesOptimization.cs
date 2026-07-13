using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class CubesOptimization : MonoBehaviour
{
 public GameObject MapParent;

 public List<string> ExistColors = new List<string>();
public List<GameObject> ObjectColors = new List<GameObject>();


// add ExistColors as gameobjects, after adding the existcolors add the colors to thier parents
 void Encapsulation()
 {
  var map = MapParent.transform;
  for (int i = 0; i < map.childCount; i++)
  {
   if (!ExistColors.Contains(map.GetChild(i).tag))
   {
    // add name to exist colors
    ExistColors.Add(map.GetChild(i).tag);
    // create object and add to objects list ( so we can add the cube from another loop to the object as childern )
    GameObject color = Instantiate(new GameObject(map.GetChild(i).tag));
    ObjectColors.Add(color);
    
   }
  }
  for (int i = 0; i < map.childCount; i++)
  {
   for (int j = 0; j < ObjectColors.Count; j++)
   {
    if (map.GetChild(i).tag == ObjectColors[j].tag)
    {
     map.GetChild(i).SetParent(ObjectColors[j].transform);
    }
   }
  }
  
 }
 
}
