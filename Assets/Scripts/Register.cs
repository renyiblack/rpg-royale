using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    public Button createButton;
    public Button cancelButton;
    public TMP_InputField emailInput;
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;

    void Start()
    {
        createButton.onClick.AddListener(OnClick);
        cancelButton.onClick.AddListener(OnCancel);
    }

    void OnClick()
    {
        emailInput.readOnly = true;
        passwordInput.readOnly = true;
        nameInput.readOnly = true;

        StartCoroutine(CreateAccount(emailInput.text, nameInput.text, passwordInput.text));
    }
    
    void OnCancel()
    {
        SceneManager.LoadScene(sceneName: "Launcher", mode: LoadSceneMode.Single);
    }

    IEnumerator CreateAccount(string login, string name, string password)
    {
        UnityWebRequest request = new UnityWebRequest("http://localhost:8080/account", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"email\": \"" + login + "\", \"name\": \"" + name + "\", \"password\": \"" + password + "\"}");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (GetErrorCode(request.responseCode) != 2)
        {
            Debug.Log(request.error);
        }
        else
        {
            SceneManager.LoadScene(sceneName: "Launcher", mode: LoadSceneMode.Single);
        }
        this.emailInput.readOnly = false;
        this.passwordInput.readOnly = false;
        this.nameInput.readOnly = false;
    }

    long GetErrorCode(long error)
    {
        return error / 100;
    }
}
