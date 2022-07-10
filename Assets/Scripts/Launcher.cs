using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    public Button loginButton;
    public Button registerButton;
    public TMP_InputField loginInput;
    public TMP_InputField passwordInput;

    void Start()
    {
        loginButton.onClick.AddListener(OnClick);
        registerButton.onClick.AddListener(OnRegisterClick);
    }

    void OnClick()
    {
        loginInput.readOnly = true;
        passwordInput.readOnly = true;

        StartCoroutine(SendAccount(loginInput.text, passwordInput.text));
    }
    
    void OnRegisterClick()
    {
        SceneManager.LoadScene(sceneName: "Register", mode: LoadSceneMode.Single);
    }

    IEnumerator SendAccount(string login, string password)
    {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/login", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"email\": \"" + login + "\", \"password\": \"" + password + "\"}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.responseCode != 200)
        {
            Debug.Log(request.error);
        }
        else
        {
            SceneManager.LoadScene(sceneName:"Lobby", mode: LoadSceneMode.Single);
        }
        this.loginInput.readOnly = false;
        this.passwordInput.readOnly = false;
    }
}
