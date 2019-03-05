using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Colyseus.Plugins.GameDevWare.Serialization;
using Colyseus.StateListener;
using UnityEngine;

namespace Colyseus
{
    public class GameClient : MonoBehaviour
    {
        Client client;
        Room room;

        public string serverName = "localhost";
        public string port = "8080";
        public string roomName = "chat";
        public string userId = "Vinh";

        // map of players
        Dictionary<string, GameObject> players = new Dictionary<string, GameObject>();

        // Use this for initialization
        IEnumerator Start()
        {
            String uri = "ws://" + serverName + ":" + port;
            Debug.Log(uri);
            client = new Client(uri, userId);
            client.OnClose += (object sender, EventArgs e) => Debug.Log("CONNECTION CLOSED");

            yield return StartCoroutine(client.Connect());
            Debug.Log("Connected to server. Client id: " + client.id);

            room = client.Join(roomName, new Dictionary<string, object>()
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

            //when other players go online or offline
//        room.Listen("players/:id", change =>
//        {
//            var playerId = change.path["id"];
//            if (change.operation == "add")
//            {
//                var value = (IDictionary<string, object>) change.value;
//                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//
//                cube.transform.position = new Vector3(Convert.ToSingle(value["x"]), Convert.ToSingle(value["y"]), 0);
//
//                Debug.Log("OnPlayerChange");
//                Debug.Log(change.operation);
//                Debug.Log(playerId);
//
//                players.Add(playerId, cube);
//            }
//            else if (change.operation == "remove")
//            {
//                GameObject cube;
//                if (!players.TryGetValue(playerId, out cube))
//                {
//                    return;
//                }
//
//                Destroy(cube);
//                players.Remove(playerId);
//            }
//        });

            //when server make players move
            room.Listen("players/:id/score", change =>
            {
                print("Operation " + change.operation);
                print("Player Id: " + change.path["id"]);
                print("New value" + change.value);

                foreach (var key in change.path.Keys)
                {
                    print(string.Format("{0}: {1}", key, change.path[key]));
                }
            });
        
            room.Listen("messages/:index", change =>
            {
                if (change.operation == "add")
                {
                    print("New message content " + change.value);
                    print("index " + change.path["index"]);
                }
            });

            room.Listen("turn", change =>
            {
                print(change.operation);

                if (change.operation == "replace")
                {
                    print("now turn " + change.value);
                }
            });

            room.Listen(this.OnChangeFallback);

            room.OnMessage += (o, e) =>
            {
                var message = (IndexedDictionary<string, object>) e.message;
            };

            int i = 0;

            while (true)
            {
                client.Recv();

                i++;

                if (i % 50 == 0)
                {
                    var data = new Dictionary<string, object>();

                    data["code"] = "chat";
                    data["content"] = "hello";
                    room.Send(data);
                }

                yield return 0;
            }
        }

        void OnChangeFallback(PatchObject change)
        {
        }

        void OnApplicationQuit()
        {
            // Make sure client will disconnect from the server
            room.Leave();
            client.Close();
        }
    }

    public struct Message
    {
    }
}