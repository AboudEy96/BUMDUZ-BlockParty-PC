using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Android.Gradle.Manifest;
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
        GameStateManager.SetState(GameState.Playing);
        StartCoroutine(DelayThenExecute(2f, _selectColorCommand));
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
        _currentCubes = mapChanger.GetCubesFromCurrentMap();

        StartCoroutine(DelayThenRun(5f, () =>
        {
            if (PhotonNetwork.IsMasterClient)
                new DestroyCubesCommand(_photonView, ChosenTag).Execute();
        }));
    }

    [PunRPC]
    private void SyncDestroyCubes(string selectedTag)
    {
        foreach (Transform cube in _currentCubes)
        {
            if (cube != null && cube.tag != selectedTag)
                cube.gameObject.SetActive(false);
        }
        _currentCubes.Clear();

        colorIndicator.Hide();

        RandomAudioPlayer.PausedOfBlocksDestroy = true;
        RandomAudioPlayer.PauseResumeAudio();

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(DelayThenExecute(3f, _nextMapCommand));
    }

    [PunRPC]
    private void SyncNextMap(int mapIndex)
    {
        mapChanger.ActivateMap(mapIndex);
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