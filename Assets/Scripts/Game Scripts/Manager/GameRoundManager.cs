using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Action = System.Action;

public class GameRoundManager : MonoBehaviourPunCallbacks
{
    [Header("References")]
    public MapChanger mapChanger;

    [Header("UI")]
    public Button startButton;
    public ColorIndicatorUI colorIndicator;

    public static string ChosenTag { get; private set; }
    public static int Score        { get; private set; }

    private PhotonView _photonView;
    private List<Transform> _currentCubes = new List<Transform>();

    private SelectColorCommand _selectColorCommand;
    private NextMapCommand _nextMapCommand;

    public static Action OnNextMapStarted;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        _selectColorCommand = new SelectColorCommand(_photonView, mapChanger);
        _nextMapCommand     = new NextMapCommand(_photonView, mapChanger);

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
            startButton.onClick.AddListener(OnStartClick);

            int firstMap = mapChanger.PickNextMapIndex();
            _photonView.RPC("SyncFirstMap", RpcTarget.AllBuffered, firstMap);
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    private void OnStartClick()
    {
        startButton.gameObject.SetActive(false);
        photonView.RPC(nameof(RPC_StartGame), RpcTarget.All);
        
        StartCoroutine(DelayThenAssignAndStart());
    }
    private IEnumerator DelayThenAssignAndStart()
    {
        yield return new WaitForSeconds(0.5f);
        AssignRandomPositions();
        StartCoroutine(DelayThenExecute(2f, _selectColorCommand));
    }
    [PunRPC]
    void SetPlayerPosition(Vector3 pos)
    {
        StartCoroutine(WaitForPlayerThenMove(pos));
    }
    
    private IEnumerator WaitForPlayerThenMove(Vector3 pos)
    {
        PhotonView targetPV = null;
        float timeout = 3f;
        float elapsed = 0f;

        while (targetPV == null && elapsed < timeout)
        {
            foreach (var pv in FindObjectsOfType<PhotonView>())
            {
                if (pv.IsMine && pv.gameObject.CompareTag("Player"))
                {
                    targetPV = pv;
                    break;
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (targetPV != null)
            targetPV.transform.position = pos;
        else
            Debug.LogWarning("SetPlayerPosition: local player not found after timeout");
    }
    [PunRPC]
    private void RPC_StartGame()
    {
        GameStateManager.SetState(GameState.Playing);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "isDead", false }
        });
    }

    [PunRPC]
    private void SyncFirstMap(int mapIndex)
    {
        mapChanger.ActivateMap(mapIndex);
        StartCoroutine(WaitForMapThenSetup());
    }

    [PunRPC]
    private void SyncSelectedColor(string selectedTag)
    {
        ChosenTag = selectedTag;
        colorIndicator.Show(selectedTag);

        StartCoroutine(WaitThenGetCubesAndDestroy(selectedTag));
    }

    private IEnumerator WaitThenGetCubesAndDestroy(string selectedTag)
    {
        yield return new WaitUntil(() => mapChanger.GetCurrentMap() != null
                                         && mapChanger.GetCurrentMap().activeInHierarchy);

        _currentCubes = mapChanger.GetCubesFromCurrentMap();

        yield return new WaitForSeconds(5f);

        if (PhotonNetwork.IsMasterClient)
        {
            new DestroyCubesCommand(_photonView, selectedTag).Execute();
            
            yield return new WaitForSeconds(3f);
            _nextMapCommand.Execute();
        }
    }

    [PunRPC]
    private void SyncDestroyCubes(string selectedTag)
    {
        Debug.Log("SyncDestroyCubes CALLED on: " + PhotonNetwork.NickName);

        var cubes = mapChanger.GetCubesFromCurrentMap();

        foreach (Transform cube in cubes)
        {
            if (cube != null && cube.tag != selectedTag)
                cube.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    private void SyncNextMap(int mapIndex)
    {
        mapChanger.ActivateMap(mapIndex);
        StartCoroutine(WaitForMapThenContinue());
    }

    void AssignRandomPositions()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            float x = Random.Range(-16f, 14f);
            float z = Random.Range(-15f, 14f);
            Vector3 randomPos = new Vector3(x, 1f, z);

            photonView.RPC("SetPlayerPosition", player, randomPos);
        }
    }

    private IEnumerator WaitForMapThenSetup()
    {
        yield return new WaitUntil(() => mapChanger.GetCurrentMap() != null);
        ColorChangeEvent.SetUpColors(mapChanger.GetCurrentMap().transform);
    }

    private IEnumerator WaitForMapThenContinue()
    {
        yield return new WaitUntil(() => mapChanger.GetCurrentMap() != null);

        Score++;
        ColorChangeEvent.SetUpColors(mapChanger.GetCurrentMap().transform);

        RandomAudioPlayer.PausedOfBlocksDestroy = false;
        RandomAudioPlayer.PauseResumeAudio();
        OnNextMapStarted?.Invoke();

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(DelayThenExecute(4.5f, _selectColorCommand));
    }

    private IEnumerator DelayThenExecute(float seconds, IGameCommand command)
    {
        yield return new WaitForSeconds(seconds);
        command.Execute();
    }

    private IEnumerator DelayThenRun(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
}