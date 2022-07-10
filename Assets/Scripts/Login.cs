using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Button loginButton;
    public TMP_InputField login;
    public TMP_InputField password;

    void Start()
    {
        loginButton.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        login.readOnly = true;
        password.readOnly = true;

        StartCoroutine(SendAccount(login.text, password.text));
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
        this.login.readOnly = false;
        this.password.readOnly = false;
    }
}
