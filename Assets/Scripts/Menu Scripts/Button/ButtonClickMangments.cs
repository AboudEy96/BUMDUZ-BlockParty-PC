using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using ColorUtility = UnityEngine.ColorUtility;
using Image = UnityEngine.UI.Image;

public class ButtonClickMangments : MonoBehaviour,IButtonClickMangment
{
    public List<Transform> characters = new List<Transform>();
     public List<GameObject> buttons = new List<GameObject>();
     public Transform theImage;
     public GameObject lightFade;
     
     [Header("For Demo Menu ")]
     public string[] demoMENU = new string[] { "Shop" };

     public Transform demoMenuImage;
     
     [Header("Main and Character Camera")]
     public Camera characterCamera;
     public Camera mainCamera;
     [Header("Player Prefab and Material")]
    // public GameObject PREFAB_PLAYER = PlayerCharacterSingletoon.Instance.CHARACTER;
    // public Material PLAYER_SKIN = PlayerCharacterSingletoon.Instance.SKIN;
    
     private string CurrentMode;
     [Header("The Menu of choose object")] public GameObject chooseModeObject;
     
     [Header("For Main Canvas and Loading Menu")] public Canvas MainCanvas;
        public Canvas LoadingCanvas;
        public Canvas ProfileCanvas;
     
        [Header("The Loading Images")]
        public List<Sprite> loadingImg = new List<Sprite>();

        [Header("SkyBox Hex To change")] public Material TheSkyBox;


        public void Awake()
     {
         SetCurrentMode("Map");
         Color a;
         string hexColor = "#808080";
         ColorUtility.TryParseHtmlString(hexColor, out a);
         TheSkyBox.SetColor("_TintColor", a);
     }
     public void SendLoading()
     {
         CanvasManager.CloseClick(); // close all canvas and show loading UI
         Image im = LoadingCanvas.GetComponentInChildren<Image>();
         int randomIndex = Random.Range(0, loadingImg.Count);
         im.sprite = loadingImg[randomIndex];
         LoadingCanvas.gameObject.SetActive(true);
     }
     
    public void Play(GameObject button)
    {
        StartCoroutine(PlayAfterLoading(button));
    }
    public IEnumerator PlayAfterLoading(GameObject button)
    {
        string buttonName = button.transform.name;
        switch (buttonName)
        {
            case "Singleplayer":
                SendLoading();
                yield return new WaitForSeconds(0.4f);
                PhotonNetwork.Disconnect();
                PhotonNetwork.OfflineMode = true;
                PhotonNetwork.CreateRoom("OfflineRoom");
                SceneManager.LoadScene("Game");
                Debug.Log("Singleplayer Mode ------");
                break;
            
        }
        
    }
    public void ShowProfile()
    {
        ProfileCanvas.gameObject.SetActive(true);
    }
    
    public void Other(GameObject button)
    {

        foreach (GameObject canvas in CanvasManager.instance.GetCanvases())
        {
            canvas.gameObject.SetActive(false);
            if (canvas.name.Contains(button.name))
            {
                Debug.Log($"Canvas: {canvas.name}, Button: {button.name}");
                canvas.gameObject.SetActive(true);
                Debug.Log($"CANVAS FOUND FOUND FOUND");
            }
            Debug.Log($"Canvas = '{canvas.name}' | Button = '{button.name}'");

        }
    }
    
    

    public void HideShowFade()
    {
        lightFade.SetActive(GetCurrentMode().Equals("Map"));
    }
    public string GetCurrentMode()
    {
        return this.CurrentMode;
    }

    public void SetCurrentMode(string mode)
    {
        this.CurrentMode = mode; 
    }

    public void ActiveCamera()
    {
        bool characterMode = GetCurrentMode().Equals("Character") ? true : false;
        characterCamera.gameObject.SetActive(characterMode);
        mainCamera.enabled = !characterMode;
    }

    public void ChangeSkyBoxHex()
    {
        bool isCharacter = GetCurrentMode() == "Character";

        StartCoroutine(ChangeSkyboxColorCoroutine(
            isCharacter ? "#808080" : "#FF4EE9",
            isCharacter ? "#FF4EE9" : "#808080",
            1f
        ));
    }
    
    IEnumerator ChangeSkyboxColorCoroutine(string fromHex, string toHex, float duration)
    {
        Color startColor;
        Color endColor;

        ColorUtility.TryParseHtmlString(fromHex, out startColor);
        ColorUtility.TryParseHtmlString(toHex, out endColor);

        float t = 0f;
        RenderSettings.skybox = TheSkyBox;

        while (t < duration)
        {
            t += Time.deltaTime;
            Color newColor = Color.Lerp(startColor, endColor, t / duration);

            TheSkyBox.SetColor("_TintColor", newColor);

            yield return null;
        }
    }
    

    public void onCloseButtonClick(GameObject ToClose)
    {
        GameObject clicked = EventSystem.current.currentSelectedGameObject;
        if (clicked == null) return;
 
        ToClose.SetActive(false);
        CanvasManager.CloseClick(); // observe to show canvas
    }
}
