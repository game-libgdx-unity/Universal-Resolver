/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.Chat;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Client : MonoBehaviour, IChatClientListener
{
    #region Constants & Variables

    /// <summary>
    /// channel to send messages
    /// </summary>
    private const string generalChannel = "all";

    private ChatClient chatClient;
    private string chatAppVersion = "0.0.0";

    /// <summary>
    /// cached id
    /// </summary>
    public string userId;

    /// <summary>
    /// cached friend ids
    /// </summary>
    private string[] friendIds;

    /// <summary>
    /// log data stream to console
    /// </summary>
    public bool EnableLogging = true;

    /// <summary>
    /// do log in on Awaken
    /// </summary>
    public bool AutoLogin = true;

    /// <summary>
    /// list status of friends
    /// </summary>
    public ReactiveDictionary<string, PlayerStatus>
        FriendStatus = new ReactiveDictionary<string, PlayerStatus>();

    /// <summary>
    /// list status message of friends
    /// </summary>
    public ReactiveDictionary<string, string> FriendStatusMsg = new ReactiveDictionary<string, string>();

    /// <summary>
    /// Current online users, will be update in real-time
    /// </summary>
    public ReactiveCollection<string> OnlineUsers = new ReactiveCollection<string>(new List<string>());

    /// <summary>
    /// A new message just was sent from this client
    /// </summary>
    public Subject<ChallengeRoomInfo> ChallengingRoomInfo = new Subject<ChallengeRoomInfo>();

    /// <summary>
    /// A new message just was sent from this client
    /// </summary>
    public Subject<Hashtable> SentMessages = new Subject<Hashtable>();

    /// <summary>
    /// A new message just was received by this client
    /// </summary>
    public Subject<Hashtable> ReceivedMessages = new Subject<Hashtable>();

    /// <summary>
    /// If a new friend request has been received
    /// </summary>
    public Subject<string> FriendRequestIncoming = new Subject<string>();

    /// <summary>
    /// If a new friend request has been removed
    /// </summary>
    public Subject<string> FriendRequestRemoval = new Subject<string>();

    /// <summary>
    /// result of friend request you just have sent
    /// </summary>
    public Subject<FriendRequestResult> FriendRequestResult = new Subject<FriendRequestResult>();

    /// <summary>
    /// If a new challenge request has been received
    /// </summary>
    public Subject<string> NewChallengeRequest = new Subject<string>();

    /// <summary>
    /// result of challenge request you just have sent
    /// </summary>
    public Subject<ChallengeRequestResult> ChallengeRequestResult = new Subject<ChallengeRequestResult>();

    /// <summary>
    /// when a friend updated their status
    /// </summary>
    public Subject<FriendStatusUpdate> FriendStatusUpdate =
        new Subject<FriendStatusUpdate>();

    /// <summary>
    /// If a new user just go online
    /// </summary>
    public Subject<string> NewUserOnline = new Subject<string>();

    /// <summary>
    /// If a new user just go online
    /// </summary>
    public Subject<string> NewUserOffline = new Subject<string>();

    /// <summary>
    ///  If user connected to chat network
    /// </summary>
    public BoolReactiveProperty IsReady = new BoolReactiveProperty();

    #endregion

    #region Unity & Photon callbacks

    protected void Start()
    {
        if (AutoLogin)
        {
            Connect(userId, null);
        }
    }

    public void OnConnected()
    {
        PlayerStatus = global::PlayerStatus.Online;

        if (friendIds != null)
        {
            //add friend to photon chat
            chatClient.AddFriends(friendIds);

            //convert friendIds to friends dictionary
            Array.ForEach(friendIds, fid => FriendStatus.Add(fid, global::PlayerStatus.Offline));
        }

        chatClient.Subscribe(generalChannel, 0, true);
    }

    void Update()
    {
        if (chatClient != null)
            chatClient.Service();
    }

    void OnDestroy()
    {
        CancelInvoke();
        IsReady.Value = false;

        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void OnDisconnected()
    {
        CancelInvoke();
        IsReady.Value = false;

        //try reconnect again
        Observable.Timer(TimeSpan.FromSeconds(1f))
            .Subscribe(_ => { Connect(this.userId, null); })
            .AddTo(gameObject);
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    #endregion

    #region Public members

    public void Connect(string userId, string[] friendIds = null)
    {
        if (chatClient != null)
        {
            return;
        }

        //get a random id if the input isn't validated
        if (string.IsNullOrEmpty(userId))
        {
            userId = UnityEngine.Random.Range(10000, 100000).ToString();
        }

        this.userId = userId;
        this.friendIds = friendIds;
        //create client for photon chat
        chatClient = new ChatClient(this, ConnectionProtocol.Tcp);
        chatClient.ChatRegion = "ASIA";

#if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
#endif

        //connect to photon chat cloud
        var authenticationValues = new ExitGames.Client.Photon.Chat.AuthenticationValues(userId);
//        authenticationValues.AuthType = ExitGames.Client.Photon.Chat.CustomAuthenticationType.None;
//        Debug.Log("Connect to app id: "+PhotonNetwork.PhotonServerSettings.ChatAppID);

        this.chatClient.Connect("72628236-d9f8-4bda-9ff8-06c314886303", "1.0",
            authenticationValues);

        //only do logging if
#if UNITY_EDITOR || DEVELOPMENT_BUILD

        if (EnableLogging)
        {
            //log new friend request to unity console
//            SentMessages.SubscribeToConsole("Sent msg").AddTo(gameObject);
//            FriendRequestIncoming.SubscribeToConsole("Received friend request from ").AddTo(gameObject);
//            NewChallengeRequest.SubscribeToConsole("Received challenge request from ").AddTo(gameObject);
//            NewUserOnline.SubscribeToConsole("NewUserOnline ").AddTo(gameObject);
//            NewUserOffline.SubscribeToConsole("NewUserOffline ").AddTo(gameObject);
//            FriendRequestResult.Select(r => r.result).SubscribeToConsole("Received Friend result ").AddTo(gameObject);
//            ChallengeRequestResult.Select(r => r.result).SubscribeToConsole("Received challenge result ")
//                .AddTo(gameObject);
//            IsReady.SubscribeToConsole("IsReady ").AddTo(gameObject);
//
//            //log current user update to unity console
//            FriendStatus.ObserveReplace()
//                .Select(arg => arg.Key + " " + arg.NewValue)
//                .SubscribeToConsole("Status updated ")
//                .AddTo(gameObject);
//
//            FriendStatusMsg.ObserveReplace()
//                .Select(arg => arg.Key + " " + arg.NewValue)
//                .SubscribeToConsole("Status msg updated ")
//                .AddTo(gameObject);
        }
#endif
    }

    public void Disconnect()
    {
        CancelInvoke();
        chatClient.Disconnect();
    }

    public ChatClient ChatClient
    {
        get { return chatClient; }
    }

    private PlayerStatus playerStatus;

    public PlayerStatus PlayerStatus
    {
        get { return playerStatus; }
        set
        {
            playerStatus = value;
            chatClient.SetOnlineStatus((int) value);
        }
    }

    public void SendMessageData(string receiver,
        byte actionId,
        Hashtable data = null,
        string message = null,
        Exception exception = null)
    {
        if (data == null)
        {
            data = new Hashtable();
        }

        if (message != null)
        {
            data[Field.Message] = message;
        }

        if (exception != null)
        {
            data[Field.Exception] = exception;
        }

        data[Field.ActionId] = actionId;
        data[Field.Sender] = this.userId;
        data[Field.Receiver] = receiver;

        SentMessages.OnNext(data);

        chatClient.SendPrivateMessage(receiver, data);
    }

    #endregion

    #region Friends

    public void AddFriends(params string[] friends)
    {
        ChatClient.AddFriends(friends);

        foreach (var user in friends)
        {
            if (FriendStatus.ContainsKey(user) == false)
            {
                FriendStatus.Add(user, global::PlayerStatus.Online);
            }
        }
    }

    public void SendFriendRequest(string userId)
    {
        SendMessageData(userId, ActionId.SendFriendRequest);
    }

    public void SendAcceptFriendRequest(string userId)
    {
        //add friend to photon chat
        chatClient.AddFriends(new[] {userId});

        //add friend id to friend list
        FriendStatus[userId] = PlayerStatus.Offline;

        //send the acceptation
        SendMessageData(userId, ActionId.AcceptFriendRequest);
    }

    public void SendDenyFriendRequest(string userId)
    {
        SendMessageData(userId, ActionId.DenyFriendRequest);
    }

    public void SetStatus(PlayerStatus status)
    {
        PlayerStatus = status;
    }

    public void SetStatus(PlayerStatus status, string msg)
    {
        playerStatus = status;
        chatClient.SetOnlineStatus((int) status, msg);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        if (FriendStatus.ContainsKey(user))
        {
            FriendStatus[user] = (PlayerStatus) status;
        }
        else
        {
            FriendStatus.Add(user, (PlayerStatus) status);
        }

        FriendStatusUpdate.OnNext(new FriendStatusUpdate(user, (PlayerStatus) status));

        if (gotMessage && !string.IsNullOrEmpty(message as string))
        {
            if (FriendStatusMsg.ContainsKey(user))
            {
                FriendStatusMsg[user] = message.ToString();
            }
            else
            {
                FriendStatusMsg.Add(user, message.ToString());
            }

            FriendStatusUpdate.OnNext(new FriendStatusUpdate(user, message.ToString()));
        }
    }

    #endregion

    #region Challenges

    public void SendChallengeRequest(string userId)
    {
        SendMessageData(userId, ActionId.SendChallengeRequest);
    }

    public void SendAcceptChallengeRequest(string userId)
    {
        SendMessageData(userId, ActionId.AcceptChallengeRequest);
    }

    public void SendDenyChallengeRequest(string userId)
    {
        SendMessageData(userId, ActionId.DenyFriendRequest);
    }

    public string CreatePhotonRoom(string userId)
    {
        if (PhotonNetwork.inRoom)
        {
            print("Cannot join while inside a room");
            PhotonNetwork.LeaveRoom();
        }

        var o = new RoomOptions();
        o.IsVisible = true;
        o.IsOpen = true;
        o.MaxPlayers = 2;
        o.PlayerTtl = 180000;
        o.PublishUserId = true;

        o.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
        };
        var roomName = Guid.NewGuid().ToString();
        PhotonNetwork.CreateRoom(roomName, o, TypedLobby.Default);
        return roomName;
    }

    #endregion

    #region Channels

    public void OnSubscribed(string channel, string[] subscribers, bool publishSubscribers, int maxSubscribers)
    {
        print("Subscribed to " + channel + " pub: " + publishSubscribers);
        OnlineUsers = new ReactiveCollection<string>(subscribers);
        if (channel == generalChannel)
        {
            IsReady.Value = true;
        }

        foreach (var user in subscribers)
        {
            NewUserOnline.OnNext(user);

            if (FriendStatus.ContainsKey(user))
            {
                FriendStatus[user] = global::PlayerStatus.Online;
            }
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
        if (channel == generalChannel && !OnlineUsers.Contains(user))
        {
            NewUserOnline.OnNext(user);
            OnlineUsers.Add(user);
        }

        FriendStatus[user] = global::PlayerStatus.Online;
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        if (channel == generalChannel)
        {
            NewUserOffline.OnNext(user);
            OnlineUsers.Remove(user);
        }

        FriendStatus[user] = global::PlayerStatus.Offline;
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //ignore messages from this id
        if (this.userId.Equals(sender))
        {
            return;
        }

        if (message.GetType() == typeof(Hashtable))
        {
            var hashtable = (Hashtable) message;

            var actionId = hashtable.GetActionId();

            switch (actionId)
            {
                case ActionId.SendFriendRequest:
                    FriendRequestIncoming.OnNext(sender);
                    break;
                case ActionId.SendChallengeRequest:
                    NewChallengeRequest.OnNext(sender);
                    break;
                case ActionId.AcceptFriendRequest:
                    FriendRequestResult.OnNext(new FriendRequestResult(sender, true));
                    break;
                case ActionId.DenyFriendRequest:
                    FriendRequestResult.OnNext(new FriendRequestResult(sender, false));
                    break;
                case ActionId.AcceptChallengeRequest:
                    ChallengeRequestResult.OnNext(new ChallengeRequestResult(sender, true));
                    break;
                case ActionId.DenyChallengeRequest:
                    ChallengeRequestResult.OnNext(new ChallengeRequestResult(sender, false));
                    break;
                case ActionId.ChallengeInfo:
                    ChallengingRoomInfo.OnNext(new ChallengeRoomInfo()
                    {
                        userId = sender,
                        roomName = (string) hashtable[ChallengeRequestParameter.RoomName],
                    });
                    break;

                default:
                    Debug.LogError("Unknown action code: " + actionId);
                    break;
            }

            ReceivedMessages.OnNext(hashtable);
        }
    }

    #endregion

    #region Empty implements

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    #endregion
}