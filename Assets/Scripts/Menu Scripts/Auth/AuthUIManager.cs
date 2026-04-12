using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuthUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject authPanel;

    [Header("Fields")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI errorText;

    [Header("Toggle Buttons")]
    public Button loginTabButton;
    public Button registerTabButton;

    [Header("Confirm Button")]
    public Button confirmButton;
    public TextMeshProUGUI confirmButtonText;

    private bool _isLoginMode = true;

    private void OnEnable()
    {
        AuthManager.OnLoginSuccess    += OnLoginSuccess;
        AuthManager.OnLoginFailed     += OnLoginFailed;
        AuthManager.OnRegisterSuccess += OnRegisterSuccess;
        AuthManager.OnRegisterFailed  += OnRegisterFailed;
    }

    private void OnDisable()
    {
        AuthManager.OnLoginSuccess    -= OnLoginSuccess;
        AuthManager.OnLoginFailed     -= OnLoginFailed;
        AuthManager.OnRegisterSuccess -= OnRegisterSuccess;
        AuthManager.OnRegisterFailed  -= OnRegisterFailed;
    }

    private void Start()
    {
        loginTabButton.onClick.AddListener(SetLoginMode);
        registerTabButton.onClick.AddListener(SetRegisterMode);
        confirmButton.onClick.AddListener(OnConfirmClick);

        SetLoginMode();
        CheckLoginState();
    }

    private void CheckLoginState()
    {
        if (AuthManager.Instance.IsLoggedIn())
            ShowMainMenu();
        else
        {
            ShowAuth();
            registerTabButton.interactable = true;
        }
    }

    #region Mode Toggle

    public void SetLoginMode()
    {
        _isLoginMode = true;
        confirmButtonText.text = "Login";
        errorText.text = "";

        loginTabButton.interactable    = false;
        registerTabButton.interactable = true;
    }

    public void SetRegisterMode()
    {
        _isLoginMode = false;
        confirmButtonText.text = "Register";
        errorText.text = "";

        loginTabButton.interactable    = true;
        registerTabButton.interactable = false;
    }

    #endregion

    #region Confirm

    public void OnConfirmClick()
    {
        errorText.text = "";

        if (string.IsNullOrEmpty(usernameField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            errorText.text = "Please fill all fields.";
            return;
        }

        if (_isLoginMode)
            AuthManager.Instance.Login(usernameField.text, passwordField.text);
        else
        {
            if (passwordField.text.Length < 8)
            {
                errorText.text = "Password must be at least 8 characters.";
                return;
            }
            AuthManager.Instance.Register(usernameField.text, passwordField.text);
        }
    }

    #endregion

    #region Panels

    private void ShowMainMenu()
    {
        authPanel.SetActive(false);
    }

    private void ShowAuth()
    {
        authPanel.SetActive(true);
    }

    public void OnLogoutClick()
    {
        AuthManager.Instance.Logout();
        ShowAuth();
        SetLoginMode();
    }

    #endregion

    #region Callbacks

    private void OnLoginSuccess()    => ShowMainMenu();
    private void OnRegisterSuccess() => ShowMainMenu();

    private void OnLoginFailed(string error)
    {
        errorText.text = "Login failed.. forget your password? contact us";
    }

    private void OnRegisterFailed(string error)
    {
        errorText.text = "Register failed: " + error;
    }

    #endregion
}