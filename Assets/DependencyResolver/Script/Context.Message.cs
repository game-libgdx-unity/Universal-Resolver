using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public static Action<Dictionary<string, object>> OnMessageReceived;
    
    public static void SendMessage(Dictionary<string, object> msg)
    {
        if (OnMessageReceived != null)
        {
            OnMessageReceived(msg);
        }
    }

    public static void Reset()
    {
        OnMessageReceived = null;
    }
}
