using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;

    public static Action OnLoginSuccess;
    public static Action<string> OnLoginFailed;
    public static Action OnRegisterSuccess;
    public static Action<string> OnRegisterFailed;
    public static Action OnLogOut;

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

    private string TransformPassword(string rawPassword)
    {
        return rawPassword + "Zx9!";
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (AuthenticationService.Instance.SessionTokenExists)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("Auto Login Success");

                await CloudSaveManager.Instance.LoadData();

                OnLoginSuccess?.Invoke(); 
            }
            catch (Exception e)
            {
                Debug.LogError("Auto Login Failed: " + e.Message);
                OnLoginFailed?.Invoke(e.Message);
            }
        }
        else
        {
            OnLoginFailed?.Invoke("Not logged in");
        }
    }

    public async void Register(string username, string password)
    {
        try
        {
            string transformedPassword = TransformPassword(password);

            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, transformedPassword);
            PlayerDataManager.Instance.SetPlayerName(username);
            PlayerDataManager.Instance.UnlockSkinFree("BUMDUZ[Colorful]");
            PlayerDataManager.Instance.UnlockSkinFree("MUMDUZ[Purple]");
            await CloudSaveManager.Instance.SaveData();
            OnRegisterSuccess?.Invoke();
            Debug.Log("Register success: " + username);
        }
        catch (Exception e)
        {
            OnRegisterFailed?.Invoke(e.Message);
            Debug.LogError("Register failed: " + e.Message);
        }
    }

    public async void Login(string username, string password)
    {
        try
        {
            string transformedPassword = TransformPassword(password);

            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, transformedPassword);
            await CloudSaveManager.Instance.LoadData();
            OnLoginSuccess?.Invoke();
            Debug.Log("Login success: " + username);
        }
        catch (Exception e)
        {
            OnLoginFailed?.Invoke(e.Message);
            Debug.LogError("Login failed: " + e.Message);
        }
    }

    public void Logout()
    {
        PlayerDataManager.Instance.ResetData();

        AuthenticationService.Instance.SignOut();
        OnLogOut?.Invoke();
    }

    public bool IsLoggedIn() => AuthenticationService.Instance.IsSignedIn;
}