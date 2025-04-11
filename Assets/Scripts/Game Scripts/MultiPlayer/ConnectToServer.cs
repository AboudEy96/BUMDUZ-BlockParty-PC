using System.Collections.Generic;
using NUnit.Framework;
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
        MainCanvas.gameObject.SetActive(false);
        Image im = LoadingCanvas.GetComponentInChildren<Image>();
        int randomIndex = Random.Range(0, images.Count);
        im.sprite = images[randomIndex];
        LoadingCanvas.gameObject.SetActive(true);
    }
    public void ConnectUsingSettings()
    {
        SendLoading();
       PhotonNetwork.ConnectUsingSettings();    
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}