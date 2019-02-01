using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Unity.Editor;
/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
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