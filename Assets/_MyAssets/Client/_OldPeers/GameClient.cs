using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using Newtonsoft.Json;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ChallengeRequestParameter : Parameter
{
    public const byte RoomName = Parameter.Data1;
    public const byte GameMode = Parameter.Data2;
}

public class ActionId
{
    public const byte SendFriendRequest = 1;
    public const byte AcceptFriendRequest = 2;
    public const byte DenyFriendRequest = 3;
    public const byte SendChallengeRequest = 4;
    public const byte AcceptChallengeRequest = 5;
    public const byte DenyChallengeRequest = 6;


    public const byte Login = 21;
    public const byte ChallengeInfo = 10;
}

public class Parameter
{
    protected const byte Data1 = 6;
    protected const byte Data2 = 7;
    protected const byte Data3 = 8;
    protected const byte Data4 = 9;
    protected const byte Data5 = 10;
}

public static class Field
{
    public const byte ActionId = 1;
    public const byte Message = 2;
    public const byte Sender = 3;
    public const byte Receiver = 4;
    public const byte Exception = 5;

    public static byte GetActionId(this Hashtable msg)
    {
        if (msg.ContainsKey(ActionId))
        {
            return (byte) msg[ActionId];
        }

        return byte.MinValue;
    }

    public static T GetData<T>(this Hashtable msg)
    {
        if (msg.ContainsKey(Message))
        {
            return (T) msg[ActionId];
        }

        return default(T);
    }

    public static string GetSenderId(this Hashtable msg)
    {
        if (msg.ContainsKey(Sender))
        {
            return (string) msg[Sender];
        }

        return null;
    }

    public static string GetReceiverId(this Hashtable msg)
    {
        if (msg.ContainsKey(Receiver))
        {
            return (string) msg[Receiver];
        }

        return null;
    }
}

[Serializable]
public struct FriendRequestResult
{
    public string userId;
    public bool result;

    public FriendRequestResult(string userId, bool result)
    {
        this.userId = userId;
        this.result = result;
    }
}

[Serializable]
public struct ChallengeRoomInfo
{
    public string userId;
    public string roomName;

    public ChallengeRoomInfo(string userId, string roomName)
    {
        this.userId = userId;
        this.roomName = roomName;
    }
}

[Serializable]
public struct ChallengeRequestResult
{
    public string userId;
    public bool result;

    public ChallengeRequestResult(string userId, bool result)
    {
        this.userId = userId;
        this.result = result;
    }
}

[Serializable]
public struct FriendStatusUpdate
{
    public string id;
    public PlayerStatus status;
    public object message;

    public FriendStatusUpdate(string id, PlayerStatus status)
    {
        this.id = id;
        this.status = status;
        message = null;
    }

    public FriendStatusUpdate(string id, object message)
    {
        this.id = id;
        this.message = message;
        status = PlayerStatus.Offline;
    }
}