using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings Panel")]
    public GameObject settingsPanel;

    public Button settingsButton;
    
    [Header("Mouse Speed")]
    public Slider mouseSpeedSlider;
    public TextMeshProUGUI mouseSpeedValueText;
    private const float MOUSE_SPEED_MIN = 10f;
    private const float MOUSE_SPEED_MAX = 300f;

    [Header("Music Volume")]
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumeValueText;

    [Header("Graphics Quality")]
    public Slider graphicsSlider;
    public TextMeshProUGUI graphicsValueText;
    public PostProcessVolume postProcessVolume;
    public List<PostProcessProfile> graphicsProfiles;

    private const string KEY_MOUSE  = "Settings_MouseSpeed";
    private const string KEY_VOLUME = "Settings_MusicVolume";
    private const string KEY_GFX    = "Settings_Graphics";

    private readonly string[] graphicsLabels = { "Low", "Medium", "High" };

    private void Start()
    {
        SetupSliders();
        LoadSettings();
        ApplyAll();
    }
#region onEnable,Disable
    private void OnEnable()
    {
        settingsButton.onClick.AddListener(ToggleSettingsPanel);
    }

    private void OnDisable()
    {
        settingsButton.onClick.RemoveListener(ToggleSettingsPanel);
    }

    private void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
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

        graphicsSlider.minValue    = 0;
        graphicsSlider.maxValue    = 2;
        graphicsSlider.wholeNumbers = true;
        graphicsSlider.onValueChanged.AddListener(OnGraphicsChanged);
    }
    #endregion

#region LoadApply Settings 
    private void LoadSettings()
    {
        mouseSpeedSlider.value  = PlayerPrefs.GetFloat(KEY_MOUSE,  100f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(KEY_VOLUME, 0.8f);
        graphicsSlider.value    = PlayerPrefs.GetInt(KEY_GFX,      1);
    }

    private void ApplyAll()
    {
        ApplyMouseSpeed(mouseSpeedSlider.value);
        ApplyMusicVolume(musicVolumeSlider.value);
        ApplyGraphics((int)graphicsSlider.value);
    }
#endregion

#region  MouseControl


    private void OnMouseSpeedChanged(float value)
    {
        ApplyMouseSpeed(value);
        PlayerPrefs.SetFloat(KEY_MOUSE, value);
        PlayerPrefs.Save();
    }

    private void ApplyMouseSpeed(float value)
    {
        CameraFollow cam = FindObjectOfType<CameraFollow>();
        if (cam != null)
            cam.rotationSpeed = value;

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

    #region  GFX
    
    private void OnGraphicsChanged(float value)
    {
        int index = (int)value;
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
            case 0: // LOW
                QualitySettings.shadows = ShadowQuality.Disable;
                QualitySettings.shadowDistance = 0f;
                break;
            case 1: // MEDIUM
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.shadowDistance = 30f;
                break;
            case 2: // HIGH
                QualitySettings.shadows = ShadowQuality.All;
                QualitySettings.shadowDistance = 80f;
                break;
        }
        
        if (postProcessVolume != null &&
            graphicsProfiles != null &&
            index < graphicsProfiles.Count &&
            graphicsProfiles[index] != null)
        {
            postProcessVolume.profile = graphicsProfiles[index];
        }

        UpdateText(graphicsValueText, graphicsLabels[index]);
    }
#endregion


    private void UpdateText(TextMeshProUGUI text, string value)
    {
        if (text != null)
            text.text = value;
    }


}