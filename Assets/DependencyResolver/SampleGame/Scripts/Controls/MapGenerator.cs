/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using App.Scripts.Boards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityIoC;
using Debug = UnityEngine.Debug;


[ProcessingOrder(1)]
public class MapGenerator : MonoBehaviour
{
    [SerializeField, Inject] private GridLayoutGroup gridLayout;
    [SerializeField, Inject] private Button btnRestart;
    [SerializeField, Inject("MapGenerator")] private RectTransform container;

//    [Prefab] private Cell cell;
    [Singleton] private IGameSolver gameSolver;
    [Singleton] private IGameBoard gameBoard;

    private GameSetting gameSetting = new GameSetting();
    private Observable<GameStatus> gameStatus = new Observable<GameStatus>();

    private void Awake()
    {
        MyDebug.EnableLogging = false;
        Context.GetDefaultInstance(this);
    }

    private void Start()
    {
        //setup game status, when it get changes
        gameStatus.Subscribe(status => { print("Game status: " + status.ToString()); })
            .AddTo(gameObject);

        //setup button restart
        if (btnRestart)
        {
            btnRestart.gameObject.SetActive(false);
            btnRestart.onClick.RemoveAllListeners();
            btnRestart.onClick.AddListener(() => { StartCoroutine(RestartRoutine()); });
        }

        //setup the layout
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gameSetting.Width;

        Context.OnResolved.Subscribe(this, obj =>
        {
            if (obj.GetType() == typeof(CellData))
            {
//                var cell = Context.Resolve<Cell>(container);
                var cell = Context.ResolveFromPool<Cell>(container);
            }
        });

        //build the board
        gameBoard.Build(gameSetting.Width, gameSetting.Height, gameSetting.MineCount);

        print("Map setup");

        //solve the game
        StartCoroutine(SolveRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        Benchmark.Start();

        Context.DisposeDefaultInstance();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart the game

        yield return null;
        yield return null;
        //now scene loading is complete

        Benchmark.Stop();
    }

    IEnumerator SolveRoutine()
    {
        yield return gameSolver.Solve(1f);

        print("Finished");
        btnRestart.gameObject.SetActive(true);
    }
}