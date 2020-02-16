using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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