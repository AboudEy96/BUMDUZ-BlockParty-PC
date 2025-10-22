using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class BlocksDestroyer : MonoBehaviourPunCallbacks
{
    public MapChanger _MapChanger;
    public GameObject map;
    public static string chosenTag;
    
    private List<Transform> allCubes = new List<Transform>();
    public static int score = 0;
    public Transform theImages;
    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient) 
        {
            Invoke("SelectRandomColor", 2f);
        }
    }

    private void ShowSelectedColor(Transform image, string tag)
    {
        foreach (Transform color in image)
        {
            if (color.CompareTag("Text") || color.CompareTag("LightON")) continue;
            color.gameObject.SetActive(color.CompareTag(tag));
        }
    }

    [PunRPC]
    private void SyncSelectedColor(string selectedTag)
    {
        chosenTag = selectedTag;
        ShowSelectedColor(theImages, chosenTag);
        Debug.Log($"Synchronized color selection: {chosenTag}");

        map = _MapChanger.maps[_MapChanger.getCurrentMapIndex()];
        allCubes.Clear();
        foreach (Transform child in map.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Cube"))
            {
                allCubes.Add(child);
            }
        }

        Invoke("DestroyCubes", 5f);
    }


    private void SelectRandomColor()
    {
        map = _MapChanger.maps[_MapChanger.getCurrentMapIndex()];
        allCubes.Clear();

        foreach (Transform child in map.transform)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Cube"))
            {
                allCubes.Add(child);
            }
        }

        HashSet<string> tags = new HashSet<string>();
        foreach (Transform cube in allCubes)
        {
            tags.Add(cube.tag);
        }

        string[] tagArray = tags.ToArray();
        string selectedTag = tagArray[Random.Range(0, tagArray.Length)];

        // إرسال اللون لكل اللاعبين عبر RPC
        _photonView.RPC("SyncSelectedColor", RpcTarget.AllBuffered, selectedTag);
    }

    [PunRPC]
    private void SyncDestroyCubes(string selectedTag)
    {
        foreach (Transform cube in allCubes)
        {
            if (cube.tag != selectedTag)
            {
                cube.gameObject.SetActive(false); 
            }
        }
        Debug.Log("Destroyed non-matching cubes on all clients");
        allCubes.Clear();
        
        // pause and run music 
        RandomAudioPlayer.PausedOfBlocksDestroy = true;
        RandomAudioPlayer.PauseResumeAudio();
        
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("ActiveNextMap", 3f);
        }
    }

    private void DestroyCubes()
    {
        
        if (PhotonNetwork.IsMasterClient) 
        {
            _photonView.RPC("SyncDestroyCubes", RpcTarget.AllBuffered, chosenTag);
        }
        
    }



    [PunRPC]
    private void SyncNextMap()
    {
        _MapChanger.runNextMap();
        map = _MapChanger.maps[_MapChanger.getCurrentMapIndex()];

        foreach (Transform mapCubes in map.transform)
        {
            mapCubes.gameObject.SetActive(true);
        }

        Debug.Log($"Map changed to: {_MapChanger.getMapName()}");
        score++;
        Debug.Log($"Current score: {score}");

        ColorChangeEvent.SetUpColors(map.transform);
// resume music  
        RandomAudioPlayer.PausedOfBlocksDestroy = false;
        RandomAudioPlayer.PauseResumeAudio();

        
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("SelectRandomColor", 3f);
        }
    }

    private void ActiveNextMap()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("SyncNextMap", RpcTarget.AllBuffered);
        }
    }
}
