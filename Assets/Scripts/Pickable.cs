using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    Connection connection = Connection.GetInstance();
    User user;

    void Start()
    {
        user = new User(PlayerPrefs.GetString("name"), PlayerPrefs.GetString("password"));
        connection.ws.OnMessage += (sender, e) =>
        {
            if (e.Data != null)
            {
                Debug.Log("Pickable: " + e.Data);
                Message message = JsonUtility.FromJson<Message>(e.Data);
                if (message.type == "collected")
                {
                    if (!message.message.Contains("already collected"))
                    {
                        Debug.Log("+1 skill");
                    }
                }
            }
        };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Contains("Player"))
        {
            connection.ws.Send("{\"messageType\": \"collect\",\"id\": null,\"email\": \"" + user.name + "\",\"room id\": \"" + PlayerPrefs.GetString("room id") + "\",\"name\": \"" + user.name + "\",\"password\": \"" + user.password + "\",\"obj name\": \"" + name + "\",\"x\": \"" + transform.position.x + "\",\"y\": \"" + transform.position.y + "\"}");
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
