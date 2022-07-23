using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Game : MonoBehaviour
{
    private User user;
    private List<Points> objects;
    WebSocket ws;

    public GameObject rockPrefab;
    public GameObject treePrefab;
    public GameObject flowerPrefab;
    public Transform prefabsParent;
    bool renderObjects = false;

    void Start()
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
                if (message.type == "joined room")
                {
                    objects = JsonConvert.DeserializeObject<List<Points>>(message.message);
                    renderObjects = true;
                }
            }
        };
        ws.Send("{\"messageType\": \"join room\",\"id\": null,\"email\": \"" + user.name + "\",\"name\": \"" + user.name + "\",\"password\": \"" + user.name + "\",\"room id\": \"" + PlayerPrefs.GetString("room id") + "\"}");

    }

    // Update is called once per frame
    void Update()
    {
        if (renderObjects)
        {
            foreach (Points point in objects)
            {
                if (point.id == 1)
                {
                    GameObject obj = Instantiate(treePrefab, new Vector3(point.x, point.y), Quaternion.identity, prefabsParent);
                }
                else if (point.id == 2)
                {
                    GameObject obj = Instantiate(rockPrefab, new Vector3(point.x, point.y), Quaternion.identity, prefabsParent);
                }
                else if (point.id == 3)
                {
                    GameObject obj = Instantiate(flowerPrefab, new Vector3(point.x, point.y), Quaternion.identity, prefabsParent);
                }
            }

            renderObjects = false;
        }
    }
}

class Points
{
    public int id;
    public int x;
    public int y;
}