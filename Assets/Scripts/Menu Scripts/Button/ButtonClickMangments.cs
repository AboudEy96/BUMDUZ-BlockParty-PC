using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class ButtonClickMangments : MonoBehaviour,IButtonClickMangment
{
    public List<Transform> characters = new List<Transform>();
     public List<Sprite> images = new List<Sprite>();
     public List<GameObject> buttons = new List<GameObject>();
     public Transform theImage;
     public GameObject lightFade;
     
     [Header("Main and Character Camera")]
     public Camera characterCamera;
     public Camera mainCamera;
     [Header("Player Prefab and Material")]
    // public GameObject PREFAB_PLAYER = PlayerCharacterSingletoon.Instance.CHARACTER;
    // public Material PLAYER_SKIN = PlayerCharacterSingletoon.Instance.SKIN;
    
     private string currentMode;
     [Header("The Menu of choose object")] public GameObject chooseModeObject;
     
     [Header("For Main Canvas and Loading Menu")] public Canvas MainCanvas;
        public Canvas LoadingCanvas;
     
        [Header("The Loading Images")]
        public List<Sprite> loadingImg = new List<Sprite>();

        
     public void Awake()
     {
         ActiveButtons();
     }
     public void SendLoading()
     {
         MainCanvas.gameObject.SetActive(false);
         Image im = LoadingCanvas.GetComponentInChildren<Image>();
         int randomIndex = Random.Range(0, loadingImg.Count);
         im.sprite = loadingImg[randomIndex];
         LoadingCanvas.gameObject.SetActive(true);
     }
     
    public void Play(GameObject button)
    {
        string buttonName = button.transform.name;
        switch (buttonName)
        {
            case "Singleplayer":
                SendLoading();
                PhotonNetwork.Disconnect();
                PhotonNetwork.OfflineMode = true;
                PhotonNetwork.CreateRoom("OfflineRoom");
                SceneManager.LoadScene("Game");
                break;
            default:
                chooseModeObject.SetActive(true);
                break;
        }
        
    }
        
    
    public void Other(GameObject button)
    {
        foreach (Sprite image in images)
        {
            if (button.name == image.name)
            {
                Image img = theImage.GetComponent<Image>();
                img.sprite = image;
                ActiveCamera();
                ActiveButtons();
                ActiveCharacter();
                HideShowFade();
            }
        }
    }

    public void HideShowFade()
    {
        lightFade.SetActive(GetCurrentMode().Equals("Map"));
    }
    public string GetCurrentMode()
    {
        Image img = theImage.GetComponent<Image>();
        currentMode = img.sprite.name;
        return currentMode;
    }

    public void ActiveCamera()
    {
        bool characterMode = GetCurrentMode().Equals("Character") ? true : false;
        characterCamera.gameObject.SetActive(characterMode);
        mainCamera.enabled = !characterMode;


    }

    public void ActiveButtons()
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(button.name.Contains(GetCurrentMode()));
        }   
    }

    public void ActiveCharacter()
    {
        foreach (Transform character in characters)
        {
            character.gameObject.SetActive(character.name.Contains(GetCurrentMode()));
                //   Vector3 location = new Vector3(2.349438f, -0.5281435f, 2.576072f);
          /*  PlayerUser player = new PlayerUserBuilder(PREFAB_PLAYER)
                .SetId(1)
                .SetName("Your Skin")
                .SetSkinMaterial(PLAYER_SKIN)
                .Build(location);*/
            
            
        }
    }
}
