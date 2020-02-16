using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventCode
{
    public const int ServerChanged = 32;
    public const int RoomUpdated = 70;
    public const int LobbyUpdated = 25;
    public const int PlayerLeftLobby = 24;
}

public enum RoomLevel
{
    Public,
    Private,
    L1200,
    L1350,
    L1500,
    L1650,
    L1800,
    L1950,
    L2100,
}

public static class Command
{
    public const int CreateTable = 71;
}
public class CheckMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var webView = GetComponent<UniWebView>();
        webView.OnMessageReceived += (view, message) =>
        {
            if (message.Path.Equals("in"))
            {
                var score = message.Args["data"];
                Debug.Log("in: " + score);
            }
            if (message.Path.Equals("out"))
            {
                var score = message.Args["data"];
                Debug.Log("out: " + score);
            }
            if (message.Path.Equals("out2"))
            {
                var score = message.Args["data"];
                Debug.Log("out2: " + score);
            }
            if (message.Path.Equals("out3"))
            {
                var score = message.Args["data"];
                Debug.Log("out3: " + score);
            }
        };
    }
}