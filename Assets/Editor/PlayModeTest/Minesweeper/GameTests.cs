using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UniRx;
using UnityIoC;

public class GameTest {
    
    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator GameTestsWithEnumeratorPasses()
    {

        MineSweeper.context = new Context(typeof(MapGenerator));
        MineSweeper.context.Resolve<MapGenerator>(LifeCycle.Singleton);

        var gameStatus =  MineSweeper.context.Resolve<ReactiveProperty<GameStatus>>(LifeCycle.Singleton);
        
        yield return new WaitUntil(()=> gameStatus.Value == GameStatus.Failed || 
                                        gameStatus.Value == GameStatus.Completed);
    }
}
