using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCharacterSingletoon : MonoBehaviour
{
    // -- Singletoon to save the player character and color --
    public static GameObject CHARACTER;
    public static Texture COLOR;
    public static Material MAT;

    private void Start()
    {
        MAT = CHARACTER.GetComponent<Renderer>().material;
        MAT.mainTexture = COLOR;
        
    }
    
}