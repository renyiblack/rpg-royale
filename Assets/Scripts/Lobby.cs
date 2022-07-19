using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class Lobby : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    WebSocket ws;
    private void Start()
    {
        ws = new WebSocket("ws://localhost:8080");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            if (e.Data != null)
            {
                Debug.Log(e.Data);
                Message message = JsonUtility.FromJson<Message>(e.Data);
                if (message.type == "lobby")
                {
                   textMeshPro.text += JsonUtility.FromJson<Message>(e.Data).message + "\n";
                }
            }
        };
    }
    private void Update()
    {
        if (ws == null)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("{\"messageType\": \"login\",\"id\": null,\"email\": \"victor\",\"name\": \"victor\",\"password\": \"123\",\"message\": \"yolo\"}");
        }

        textMeshPro.ForceMeshUpdate();
    }
}

class Message
{
    public string type;
    public string message;
}
