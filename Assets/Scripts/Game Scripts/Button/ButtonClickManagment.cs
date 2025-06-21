using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonClickManagment : MonoBehaviourPunCallbacks
{
    [Header("The Loading Images")]
    public List<Sprite> images = new List<Sprite>();
    
    public List<Canvas> MainCanvas;
    public Canvas LoadingCanvas;
    
    public void SendLoading()
    {
        foreach (Canvas canvas in MainCanvas)
        {
            canvas.gameObject.SetActive(false);
        }
        Image im = LoadingCanvas.GetComponentInChildren<Image>();
        int randomIndex = Random.Range(0, images.Count);
        im.sprite = images[randomIndex];
        LoadingCanvas.gameObject.SetActive(true);
    }
    
    public IEnumerator MoveToLobby()
    {
        SendLoading();
        PhotonNetwork.LeaveRoom();
        yield return new WaitForSeconds(2.0f);

    }
    public void OnPlayButtonClicked()
    {
        StartCoroutine(MoveToLobby());
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }
}