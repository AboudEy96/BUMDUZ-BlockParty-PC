using System;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerMaterial : MonoBehaviour
{
   public List<Material> _skinMaterials = new List<Material>();

   public static SyncPlayerMaterial instance;
   private void Awake()
   {
      if (instance != null && instance != this)
      {
         Destroy(this.gameObject);
         return;
      }
      
      instance = this;
      DontDestroyOnLoad(gameObject);
   }
   public SyncPlayerMaterial GetInstance()
   {
      return instance;
   }
   public Material GetMaterialByName(string name)
   {
      foreach (var mat in _skinMaterials)
      {
         if (mat.name == name)
         {
            return mat;
         }
      }
      return null;
   }

}