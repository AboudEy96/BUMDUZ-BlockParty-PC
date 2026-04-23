using System.Collections;
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
    public AudioSource musicMainMenu;

    [Header("Graphics Quality Buttons")]
    public Button buttonLow;
    public Button buttonMedium;
    public Button buttonHigh;
    public List<PostProcessProfile> graphicsProfiles;

    [Header("Camera Pitch")]
    public Button buttonPitchToggle;
    public TextMeshProUGUI pitchToggleText;

    [Header("Save, Close BUTTONS")]
    public Button buttonClose;

    private PostProcessVolume postProcessVolume;
    private List<PostProcessProfile> _runtimeProfiles;

    private const string KEY_MOUSE  = "Settings_MouseSpeed";
    private const string KEY_VOLUME = "Settings_MusicVolume";
    private const string KEY_GFX    = "Settings_Graphics";
    private const string KEY_PITCH  = "Settings_CameraPitch";

    private const string MAIN_SCENE_NAME = "MainScene";

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
        SetupPitchButton();
        LoadSettings();
        ApplyAll();
    }

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

        if (scene.name == MAIN_SCENE_NAME)
            RebindUIReferences();

        ApplyGraphics(PlayerPrefs.GetInt(KEY_GFX, 1));
        ApplyMusicVolume(PlayerPrefs.GetFloat(KEY_VOLUME, 0.8f));
    }

    private void RebindUIReferences()
    {
        var sliders = FindObjectsOfType<Slider>(true);
        foreach (var s in sliders)
        {
            if (s.name == "MouseSpeedSlider")
            {
                mouseSpeedSlider = s;
                mouseSpeedSlider.minValue = MOUSE_SPEED_MIN;
                mouseSpeedSlider.maxValue = MOUSE_SPEED_MAX;
                mouseSpeedSlider.onValueChanged.RemoveAllListeners();
                mouseSpeedSlider.onValueChanged.AddListener(OnMouseSpeedChanged);
                mouseSpeedSlider.value = PlayerPrefs.GetFloat(KEY_MOUSE, 100f);
            }
            if (s.name == "MusicVolumeSlider")
            {
                musicVolumeSlider = s;
                musicVolumeSlider.minValue = 0f;
                musicVolumeSlider.maxValue = 1f;
                musicVolumeSlider.onValueChanged.RemoveAllListeners();
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
                musicVolumeSlider.value = PlayerPrefs.GetFloat(KEY_VOLUME, 0.8f);
            }
        }

        var texts = FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var t in texts)
        {
            if (t.name == "MouseSpeedValueText")  mouseSpeedValueText  = t;
            if (t.name == "MusicVolumeValueText") musicVolumeValueText = t;
            if (t.name == "PitchToggleText")      pitchToggleText      = t;
        }

        var buttons = FindObjectsOfType<Button>(true);
        foreach (var b in buttons)
        {
            switch (b.name)
            {
                case "SettingsButton":
                    settingsButton = b;
                    settingsButton.onClick.RemoveAllListeners();
                    settingsButton.onClick.AddListener(ToggleSettingsPanel);
                    break;
                case "CloseButton":
                    buttonClose = b;
                    buttonClose.onClick.RemoveAllListeners();
                    buttonClose.onClick.AddListener(CloseSettingsPanel);
                    break;
                case "LowButton":
                    buttonLow = b;
                    buttonLow.onClick.RemoveAllListeners();
                    buttonLow.onClick.AddListener(() => OnGraphicsChanged(0));
                    break;
                case "MediumButton":
                    buttonMedium = b;
                    buttonMedium.onClick.RemoveAllListeners();
                    buttonMedium.onClick.AddListener(() => OnGraphicsChanged(1));
                    break;
                case "HighButton":
                    buttonHigh = b;
                    buttonHigh.onClick.RemoveAllListeners();
                    buttonHigh.onClick.AddListener(() => OnGraphicsChanged(2));
                    break;
                case "PitchToggleButton":
                    buttonPitchToggle = b;
                    buttonPitchToggle.onClick.RemoveAllListeners();
                    buttonPitchToggle.onClick.AddListener(OnPitchToggleClicked);
                    break;
            }
        }

        settingsPanel = FindInactiveObjectByName("SettingsPanel");
        if (settingsPanel == null)
        {
            var obj = GameObject.Find("SettingsPanel");
            if (obj != null) settingsPanel = obj;
        }

        var audioSource = FindObjectOfType<AudioSource>();
        if (audioSource != null) musicMainMenu = audioSource;

        ApplyAll();
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        foreach (GameObject root in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            var result = FindInChildren(root.transform, name);
            if (result != null) return result;
        }
        return null;
    }

    private GameObject FindInChildren(Transform parent, string name)
    {
        if (parent.name == name) return parent.gameObject;
        foreach (Transform child in parent)
        {
            var result = FindInChildren(child, name);
            if (result != null) return result;
        }
        return null;
    }

    private void SetupSliders()
    {
        if (mouseSpeedSlider != null)
        {
            mouseSpeedSlider.minValue = MOUSE_SPEED_MIN;
            mouseSpeedSlider.maxValue = MOUSE_SPEED_MAX;
            mouseSpeedSlider.onValueChanged.AddListener(OnMouseSpeedChanged);
        }

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }
    }

    private void SetupGraphicsButtons()
    {
        if (buttonLow    != null) buttonLow.onClick.AddListener(()    => OnGraphicsChanged(0));
        if (buttonMedium != null) buttonMedium.onClick.AddListener(() => OnGraphicsChanged(1));
        if (buttonHigh   != null) buttonHigh.onClick.AddListener(()   => OnGraphicsChanged(2));
    }

    private void SetupPitchButton()
    {
        if (buttonPitchToggle != null)
        {
            buttonPitchToggle.onClick.RemoveAllListeners();
            buttonPitchToggle.onClick.AddListener(OnPitchToggleClicked);
        }
        ApplyPitch(PlayerPrefs.GetInt(KEY_PITCH, 0) == 1);
    }

    private void OnPitchToggleClicked()
    {
        bool current = PlayerPrefs.GetInt(KEY_PITCH, 0) == 1;
        bool next    = !current;

        PlayerPrefs.SetInt(KEY_PITCH, next ? 1 : 0);
        PlayerPrefs.Save();

        ApplyPitch(next);
        CameraFollow cam = FindObjectOfType<CameraFollow>();
        if (cam != null) cam.SetPitchEnabled(next);
    }

    private void ApplyPitch(bool enabled)
    {
        if (pitchToggleText == null) return;
        pitchToggleText.text = enabled ? "T" : "X";
    }

    private void LoadSettings()
    {
        if (mouseSpeedSlider  != null) mouseSpeedSlider.value  = PlayerPrefs.GetFloat(KEY_MOUSE,  100f);
        if (musicVolumeSlider != null) musicVolumeSlider.value = PlayerPrefs.GetFloat(KEY_VOLUME, 0.8f);
    }

    private void ApplyAll()
    {
        ApplyMouseSpeed(PlayerPrefs.GetFloat(KEY_MOUSE, 100f));
        ApplyMusicVolume(PlayerPrefs.GetFloat(KEY_VOLUME, 0.8f));
        ApplyGraphics(PlayerPrefs.GetInt(KEY_GFX, 1));
        ApplyPitch(PlayerPrefs.GetInt(KEY_PITCH, 0) == 1);
    }

    public void ToggleSettingsPanel()
    {
        if (settingsPanel == null)
        {
            Debug.LogError("SettingsManager: settingsPanel is NULL!");
            return;
        }
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

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

    private void OnMusicVolumeChanged(float value)
    {
        ApplyMusicVolume(value);
        PlayerPrefs.SetFloat(KEY_VOLUME, value);
        PlayerPrefs.Save();
    }

    private void ApplyMusicVolume(float value)
    {
        if (musicMainMenu != null)
            musicMainMenu.volume = value;

        UpdateText(musicVolumeValueText, $"{Mathf.RoundToInt(value * 100)}%");
    }

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
            _runtimeProfiles  != null &&
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
            ao.enabled.value = index == 2;

        if (postProcessVolume.profile.TryGetSettings(out Bloom bloom))
        {
            bloom.enabled.value = index != 0;
            bloom.intensity.value = index == 1 ? 0.5f : 1f;
        }

        if (postProcessVolume.profile.TryGetSettings(out Vignette vignette))
            vignette.enabled.value = index != 0;
    }

    private void HighlightActiveButton(int index)
    {
        if (buttonLow    != null) buttonLow.interactable    = index != 0;
        if (buttonMedium != null) buttonMedium.interactable = index != 1;
        if (buttonHigh   != null) buttonHigh.interactable   = index != 2;
    }

    private void UpdateText(TextMeshProUGUI text, string value)
    {
        if (text != null)
            text.text = value;
    }
}