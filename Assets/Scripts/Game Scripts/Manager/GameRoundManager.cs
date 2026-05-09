using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
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
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "isDead", false }
        });
    }

    private void OnStartClick()
    {
        startButton.gameObject.SetActive(false);
        MoveController.SetActiveAll(false);
        StartCoroutine(DelayStartGame());
    }

    IEnumerator DelayStartGame()
    {
        for (int i = 3; i > 0; i--)
        {
            Debug.Log($"game starting in : {i}");
            Title.Instance.SetTitle( $"Game Starting in:<color=red>{i}</color>");
            yield return new WaitForSeconds(1f);
        }
        Title.Instance.SetTitle($"<color=red>RUN!</color>", true);
        photonView.RPC(nameof(RPC_StartGame), RpcTarget.All);
    }
    [PunRPC]
    private void RPC_StartGame()
    {
        GameStateManager.SetState(GameState.Playing);
        
        StartCoroutine(MoveLocalPlayerToRandomPosition());

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(DelayThenExecute(2f, _selectColorCommand));
    }

    private IEnumerator MoveLocalPlayerToRandomPosition()
    {
        PhotonView localPlayer = null;
        float elapsed = 0f;

        while (localPlayer == null && elapsed < 3f)
        {
            foreach (var pv in FindObjectsOfType<PhotonView>())
            {
                if (pv.IsMine && pv.gameObject.CompareTag("Player"))
                {
                    localPlayer = pv;
                    break;
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (localPlayer != null)
        {
            float x = Random.Range(-16f, 14f);
            float z = Random.Range(-15f, 14f);
            localPlayer.transform.position = new Vector3(x, 1f, z);
        }
        else
        {
            Debug.LogWarning("player not found to move");
        }
        MoveController.SetActiveAll(true);
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
        RandomAudioPlayer.PausedOfBlocksDestroy = true;
        RandomAudioPlayer.PauseResumeAudio();
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
        colorIndicator.Hide();
        StartCoroutine(WaitForMapThenContinue());
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