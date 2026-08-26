using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Image = UnityEngine.UI.Image;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [Header("The Loading Images")]
    public List<Sprite> images = new List<Sprite>();

    public Canvas MainCanvas;
    public Canvas LoadingCanvas;

    public void SendLoading()
    {
        CanvasManager.CloseClick();
        Image im = LoadingCanvas.GetComponentInChildren<Image>();
        int randomIndex = Random.Range(0, images.Count);
        im.sprite = images[randomIndex];
        LoadingCanvas.gameObject.SetActive(true);
    }

    public void ConnectUsingSettings()
    { 
        StartCoroutine(ConnectAfterLoading());
    }

    public IEnumerator ConnectAfterLoading()
    {
        SendLoading();
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu";
        PhotonNetwork.ConnectUsingSettings();

        
    }
    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.OfflineMode)
        {
            SceneManager.LoadScene("Lobby");
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}