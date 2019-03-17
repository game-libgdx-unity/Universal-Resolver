using System;
using System.Collections;
using System.Collections.Generic;
using Colyseus;
using Colyseus.Plugins.GameDevWare.Serialization;
using Colyseus.StateListener;
using Firebase.Database;
using UniRx;
using UnityEngine;

namespace Colyseus
{
    public class ColyseusRoom : MonoBehaviour
    {
        [Singleton] public ColyseusClient colyseusClient;

        public bool isPersistent = false;
        public Room room;
        public string roomName = "chat";


        // Use this for initialization
        void Start()
        {
            //set persistent
            if (isPersistent)
            {
                DontDestroyOnLoad(this);
            }
            
            //every time the client is connected
            colyseusClient.isReady.Where(b => b).Subscribe(_ =>
                {
                    room = colyseusClient.CreateRoom(roomName, new Dictionary<string, object>()
                    {
                        {"create", true}
                    });

                    room.OnReadyToConnect += (o, e) =>
                    {
                        Debug.Log("Ready to connect to room!");
                        StartCoroutine(room.Connect());
                    };

                    room.OnJoin += (o, e) => { Debug.Log("Joined room successfully."); };

                    room.OnStateChange += (o, e) =>
                    {
                        if (e.isFirstState)
                        {
                            //get existing players
                            var playerList = (List<object>) e.state["players"];
                            print("Existing players: " + playerList.Count);
                        }
                    };

                    room.Listen("messages/:index", change =>
                    {
                        if (change.operation == "add")
                        {
                            print("New message content " + change.value);
                            print("index " + change.path["index"]);
                        }
                    });

                    room.Listen("players/:id/score", data =>
                    {
                        var playerId = data.path["id"];
                        var change = data.value;
					
                        Debug.LogFormat("{0} has {1} score", playerId, change);
                    });

                    room.Listen(this.OnChangeFallback);

                    room.OnMessage += (o, e) =>
                    {
                        var message = (IndexedDictionary<string, object>) e.message;
                    };
                })
                .AddTo(this);
        }

        public void SendMessage(object operation, object content)
        {
            var data = new IndexedDictionary<string, object>();
            data["operation"] = operation;
            data["content"] = content;
            room.Send(data);
        }

        public IObservable<Unit> ObserverOnJoin()
        {
            return Observable.FromEventPattern<EventHandler, EventArgs>(
                    h => h.Invoke,
                    h => room.OnJoin += h,
                    h => room.OnJoin -= h)
                .Select(arg => Unit.Default);
        }


        public IObservable<RoomUpdateEventArgs> ObserverStateChanges()
        {
            return Observable.FromEventPattern<EventHandler<RoomUpdateEventArgs>, RoomUpdateEventArgs>(
                    h => h.Invoke,
                    h => room.OnStateChange += h,
                    h => room.OnStateChange -= h)
                .SkipUntil(ObserverOnJoin())
                .Select(arg => arg.EventArgs);
        }

        public IObservable<IndexedDictionary<string, object>> ObserverNewMessages()
        {
            return Observable.FromEventPattern<EventHandler<MessageEventArgs>, MessageEventArgs>(
                    h => h.Invoke,
                    h => room.OnMessage += h,
                    h => room.OnMessage -= h)
                .Select(arg => arg.EventArgs.message)
                .Cast<object, IndexedDictionary<string, object>>();
        }

        void OnChangeFallback(PatchObject change)
        {
        }
        
        void OnDestroy()
        {
            // Make sure client will disconnect from the server
            room.Leave();
        }
    }
}