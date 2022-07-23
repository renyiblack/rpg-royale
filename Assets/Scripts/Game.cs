using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class Game : MonoBehaviour
{
    private User user;
    private List<Points> objects;
    Connection connection = Connection.GetInstance();

    public GameObject rockPrefab;
    public GameObject treePrefab;
    public GameObject flowerPrefab;
    public GameObject playerPrefab;
    public GameObject oponentPrefab;
    public Transform prefabsParent;
    public Camera cam;
    bool renderObjects = false;
    bool renderPlayer = false;
    bool renderOponent = false;
    List<int> playerPosition;
    List<int> oponentPosition;

    void Start()
    {
        user = new User(PlayerPrefs.GetString("name"), PlayerPrefs.GetString("password"));
        connection.ws.OnMessage += (sender, e) =>
        {
            if (e.Data != null)
            {
                Debug.Log("game: " + e.Data);
                Message message = JsonUtility.FromJson<Message>(e.Data);
                if (message.type == "joined room")
                {
                    objects = JsonConvert.DeserializeObject<List<Points>>(message.message);
                    renderObjects = true;
                }
                if (message.type.Contains("position"))
                {
                    if (message.type.EndsWith(user.name))
                    {
                        playerPosition = JsonConvert.DeserializeObject<List<int>>(message.message);
                        renderPlayer = true;
                    }
                    else
                    {
                        oponentPosition = JsonConvert.DeserializeObject<List<int>>(message.message);
                        renderOponent = true;
                    }
                }
            }
        };
        connection.ws.Send("{\"messageType\": \"join room\",\"id\": null,\"email\": \"" + user.name + "\",\"name\": \"" + user.name + "\",\"password\": \"" + user.password + "\",\"room id\": \"" + PlayerPrefs.GetString("room id") + "\"}");
    }

    // Update is called once per frame
    void Update()
    {
        if (renderPlayer)
        {
            GameObject player = Instantiate(playerPrefab, new Vector3(playerPosition[0], playerPosition[1]), Quaternion.identity, prefabsParent);
            cam.transform.SetParent(player.transform);
            cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -15);
            renderPlayer = false;
        }
        if (renderOponent)
        {
            Instantiate(oponentPrefab, new Vector3(oponentPosition[0], oponentPosition[1], 0), Quaternion.identity, prefabsParent);
            renderOponent = false;
        }
        if (renderObjects)
        {
            foreach (Points point in objects)
            {
                if (point.id == 1)
                {
                    Instantiate(treePrefab, new Vector3(point.x, point.y, 0), Quaternion.identity, prefabsParent);
                }
                else if (point.id == 2)
                {
                    Instantiate(rockPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity, prefabsParent);
                }
                else if (point.id == 3)
                {
                    Instantiate(flowerPrefab, new Vector3(point.x, point.y, 0), Quaternion.identity, prefabsParent);
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