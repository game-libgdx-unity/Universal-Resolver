using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using Facebook.Unity;
using Firebase.Database;
using UniRx;
using UnityEngine.Networking;

public class FirebaseManager
{
    [Serializable]
    public class SendFriendRequest
    {
        public long created;
    }

    [Serializable]
    public class UserId
    {
        public string user_id;
    }
}