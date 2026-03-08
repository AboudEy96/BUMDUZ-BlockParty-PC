using System;
using System.Collections.Generic;
using UnityEngine;

public class SyncPlayerMaterial : MonoBehaviour
{
   public List<Material> _skinMaterials = new List<Material>();
   public List<Material> _MUMDUZMaterials = new List<Material>();
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
   public Material GetMaterialByName(string name, string playerObjectName)
   {
      List<Material> materials = null;
      
      // if the player object's name is MUMDUZ make materials = MUMDUZMaterials , else if BUMDUZ makt it _skinmaterials
      switch (playerObjectName)
      {
         case string s when s.StartsWith("BUMDUZ"):
            materials = _skinMaterials;
            break;

         case string s when s.StartsWith("MUMDUZ"):
            materials = _MUMDUZMaterials;
            break;
      }
      if (materials == null)
         return null;

      
      foreach (var mat in materials)
      {
         if (mat.name == name)
         {
            return mat;
         } 
      }
      return null;
   }

}