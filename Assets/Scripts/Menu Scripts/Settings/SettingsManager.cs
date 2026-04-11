using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Settings Panel")]
    public GameObject settingsPanel;
    public Button settingsButton;
    
    [Header("Mouse Speed")]
    public Slider mouseSpeedSlider;
    public TextMeshProUGUI mouseSpeedValueText;
    private const float MOUSE_SPEED_MIN = 10f;
    private const float MOUSE_SPEED_MAX = 600f;

    [Header("Music Volume")]
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeValueText;

    [Header("Graphics Quality Buttons")]
    public Button buttonLow;
    public Button buttonMedium;
    public Button buttonHigh;
    public List<PostProcessProfile> graphicsProfiles;

    [Header("Save, Close BUTTONS")]
    public Button buttonClose;

    private PostProcessVolume postProcessVolume;
    private List<PostProcessProfile> _runtimeProfiles;

    private const string KEY_MOUSE  = "Settings_MouseSpeed";
    private const string KEY_VOLUME = "Settings_MusicVolume";
    private const string KEY_GFX    = "Settings_Graphics";

    private readonly string[] graphicsLabels = { "Low", "Medium", "High" };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetupSliders();
        SetupGraphicsButtons();
        SetUpButtonsListeners();
        LoadSettings();
        ApplyAll();
    }

    #region OnEnable, OnDisable
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        postProcessVolume = FindObjectOfType<PostProcessVolume>();

        if (_runtimeProfiles == null || _runtimeProfiles.Count == 0)
        {
            _runtimeProfiles = new List<PostProcessProfile>();
            foreach (var profile in graphicsProfiles)
                _runtimeProfiles.Add(Instantiate(profile));
        }

        ApplyGraphics(PlayerPrefs.GetInt(KEY_GFX, 1));
    }
    #endregion

    #region Buttons Setup
    void SetUpButtonsListeners()
    {
        settingsButton.onClick.AddListener(() => settingsPanel.SetActive(!settingsPanel.activeSelf));
        buttonClose.onClick.AddListener(() => settingsPanel.SetActive(false));
    }
    #endregion

    #region Setup
    private void SetupSliders()
    {
        mouseSpeedSlider.minValue = MOUSE_SPEED_MIN;
        mouseSpeedSlider.maxValue = MOUSE_SPEED_MAX;
        mouseSpeedSlider.onValueChanged.AddListener(OnMouseSpeedChanged);

        musicVolumeSlider.minValue = 0f;
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }

    private void SetupGraphicsButtons()
    {
        buttonLow.onClick.AddListener(()    => OnGraphicsChanged(0));
        buttonMedium.onClick.AddListener(() => OnGraphicsChanged(1));
        buttonHigh.onClick.AddListener(()   => OnGraphicsChanged(2));
    }
    #endregion

    #region Load, Apply Settings
    private void LoadSettings()
    {
        mouseSpeedSlider.value  = PlayerPrefs.GetFloat(KEY_MOUSE,  100f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(KEY_VOLUME, 0.8f);
    }

    private void ApplyAll()
    {
        ApplyMouseSpeed(mouseSpeedSlider.value);
        ApplyMusicVolume(musicVolumeSlider.value);
        ApplyGraphics(PlayerPrefs.GetInt(KEY_GFX, 1));
    }
    #endregion

    #region Mouse Control
    private void OnMouseSpeedChanged(float value)
    {
        ApplyMouseSpeed(value);
        PlayerPrefs.SetFloat(KEY_MOUSE, value);
        PlayerPrefs.Save();
    }

    private void ApplyMouseSpeed(float value)
    {
        UpdateText(mouseSpeedValueText, Mathf.RoundToInt(value).ToString());
    }
    #endregion

    #region Volume
    private void OnMusicVolumeChanged(float value)
    {
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat(KEY_VOLUME, value);
        PlayerPrefs.Save();
    }

    private void ApplyMusicVolume(float value)
    {
        if (RandomAudioPlayer.Instance != null)
            RandomAudioPlayer.Instance.setVolume(value);

        UpdateText(musicVolumeValueText, $"{Mathf.RoundToInt(value * 100)}%");
    }
    #endregion

    #region GFX
    private void OnGraphicsChanged(int index)
    {
        ApplyGraphics(index);
        PlayerPrefs.SetInt(KEY_GFX, index);
        PlayerPrefs.Save();
    }

    private void ApplyGraphics(int index)
    {
        index = Mathf.Clamp(index, 0, 2);

        QualitySettings.SetQualityLevel(index, true);

        switch (index)
        {
            case 0:
                QualitySettings.shadows = ShadowQuality.Disable;
                QualitySettings.shadowDistance = 0f;
                break;
            case 1:
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.shadowDistance = 15f;
                break;
            case 2:
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowDistance = 40f;
                break;
        }

        if (postProcessVolume != null &&
            _runtimeProfiles != null &&
            index < _runtimeProfiles.Count &&
            _runtimeProfiles[index] != null)
        {
            postProcessVolume.profile = _runtimeProfiles[index];
            ApplyPostProcessEffects(index);
        }

        HighlightActiveButton(index);
    }

    private void ApplyPostProcessEffects(int index)
    {
        if (postProcessVolume == null) return;

        if (postProcessVolume.profile.TryGetSettings(out AmbientOcclusion ao))
            ao.active = index == 2;

        if (postProcessVolume.profile.TryGetSettings(out Bloom bloom))
            bloom.active = index != 0;

        if (postProcessVolume.profile.TryGetSettings(out Vignette vignette))
            vignette.active = index != 0;
    }

    private void HighlightActiveButton(int index)
    {
        buttonLow.interactable    = index != 0;
        buttonMedium.interactable = index != 1;
        buttonHigh.interactable   = index != 2;
    }
    #endregion

    private void UpdateText(TextMeshProUGUI text, string value)
    {
        if (text != null)
            text.text = value;
    }
}