using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WebSocketSharp;

public class Lobby : MonoBehaviour
{
    public Button send;
    public TextMeshProUGUI chatHistory;
    public TextMeshProUGUI playerMessage;
    public GameObject roomPrefab;
    public Transform roomParent;
    public Button createRoom;
    private Vector3 nextRoomPos = new Vector3(410, 415, 0);
    private bool update = false;
    private string roomName = "";
    private User user;

    WebSocket ws;
    private void Start()
    {
        user = new User(PlayerPrefs.GetString("name"), PlayerPrefs.GetString("password"));
        ws = new WebSocket("ws://localhost:8081");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            if (e.Data != null)
            {
                Debug.Log(e.Data);
                Message message = JsonUtility.FromJson<Message>(e.Data);
                if (message.type == "lobby")
                {
                    chatHistory.text += JsonUtility.FromJson<Message>(e.Data).message + "\n";
                }
                else if (message.type == "created room")
                {
                    update = true;
                    roomName = message.message;

                }
            }
        };
        ws.Send("{\"messageType\": \"login\",\"id\": null,\"email\": \"" + user.name + "\",\"name\": \"" + user.name + "\",\"password\": \"" + user.password + "\",\"message\": \"yolo\"}");
        send.onClick.AddListener(onSendMessage);
        createRoom.onClick.AddListener(onCreateRoom);
    }
    private void Update()
    {
        if (update)
        {
            GameObject obj = Instantiate(roomPrefab, nextRoomPos, Quaternion.identity, roomParent);
            obj.GetComponentInChildren<TextMeshProUGUI>().text += roomName;
            obj.GetComponentInChildren<Button>().onClick.AddListener(() => onClick(roomName));
            nextRoomPos = new Vector3(0, nextRoomPos.y + 35, 0);
            update = false;
        }

        chatHistory.ForceMeshUpdate();
        if (ws == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("{\"messageType\": \"login\",\"id\": null,\"email\": \"" + user.name + "\",\"name\": \"" + user.name + "\",\"password\": \"" + user.password + "\",\"message\": \"yolo\"}");
        }
    }

    private void onSendMessage()
    {
        ws.Send("{\"messageType\": \"lobby\",\"id\": null,\"email\": \"" + user.name + "\",\"name\": \"" + user.name + "\",\"password\": \"" + user.password + "\",\"message\":\"" + playerMessage.text + "\"}");
    }

    void onCreateRoom()
    {
        ws.Send("{\"messageType\": \"create room\",\"id\": null,\"email\": \"" + user.name + "\",\"name\": \"" + user.name + "\",\"password\": \"" + user.name + "\"}");
    }

    void onClick(string roomId)
    {
        PlayerPrefs.SetString("room id", roomId);
        SceneManager.LoadScene(sceneName: "Game", mode: LoadSceneMode.Single);
    }
}

class Message
{
    public string type;
    public string message;
}

class User
{
    public string name;
    public string password;

    public User(string name, string password)
    {
        this.name = name;
        this.password = password;
    }
}
