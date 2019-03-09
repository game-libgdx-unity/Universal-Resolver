using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Colyseus.Plugins.GameDevWare.Serialization;
using Colyseus.StateListener;
using UnityEngine;
using UniRx;


namespace Colyseus
{
    public class ColyseusClient : MonoBehaviour
    {
        Client client;

        public string serverName = "localhost";
        public string port = "8080";
        public string userId = "Vinh";

        [Transient] public BoolReactiveProperty isReady;
        [Transient] public BoolReactiveProperty autoReconnect;

        // Use this for initialization
        void Start()
        {
            var uri = string.Format("ws://{0}:{1}", serverName, port);
            client = new Client(uri, userId);

            client.OnClose += (sender, e) =>
            {
                isReady.Value = false;
                Debug.Log("CONNECTION CLOSED");
            };

            StartCoroutine(Connect());

            isReady.SubscribeToConsole("Client connected");
        }

        IEnumerator Connect()
        {
            yield return StartCoroutine(client.Connect());
            isReady.Value = true;
        }

        void OnApplicationQuit()
        {
            client.Close();
        }

        public Room CreateRoom(string roomName, Dictionary<string, object> param)
        {
            return client.Join(roomName, param);
        }

        void Update()
        {
            if (isReady.Value == false)
            {
                return;
            }
            
            client.Recv();
        }
    }
}