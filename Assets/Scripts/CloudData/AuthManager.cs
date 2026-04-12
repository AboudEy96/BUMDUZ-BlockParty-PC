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
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            PlayerDataManager.Instance.SetPlayerName(username);
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
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
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
        AuthenticationService.Instance.SignOut();
    }

    public bool IsLoggedIn() => AuthenticationService.Instance.IsSignedIn;
}
