using UniRx;
using UnityEngine;
using UnityIoC;

namespace Colyseus.Samples
{
	public class TestColyseusClient : MonoBehaviour
	{
		[Singleton] private ColyseusRoom room;
		
		void Start ()
		{
			MyDebug.EnableLogging = false;
			
			new AssemblyContext(this);

//			room.ObserverDataChange("players/:id/score")
//				.SkipUntil(room.ObserverOnJoin())
//				.Subscribe(data =>
//				{
//					var playerId = data.path["id"];
//					var change = data.value;
//					
//					Debug.LogFormat("{0} has {1} score", playerId, change);
//				})
//				.AddTo(this);
//			
//			room.ObserverDataChange("turn")
//				.SkipUntil(room.ObserverOnJoin())
//				.Subscribe(data =>
//				{
//					print("now turn " + data.value);
//				})
//				.AddTo(this);
			
//			room.SendMessage("code",);
		}
	}
}
