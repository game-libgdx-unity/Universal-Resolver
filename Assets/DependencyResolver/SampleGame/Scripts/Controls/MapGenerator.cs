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

    [SerializeField, Inject("MapGenerator")]
    private RectTransform container;

//    [Prefab] private Cell cell;
    [Singleton] private IGameSolver gameSolver;
    [Singleton] private IGameBoard gameBoard;
    [Singleton, Override] private Observable<GameStatus> gameStatus;

    private GameSetting gameSetting = new GameSetting();

    private void Awake()
    {
        MyDebug.EnableLogging = false;
        Context.Setting.CreateViewFromPool = true;
        Context.GetDefaultInstance(this);
    }

    private void Start()
    {
        //setup game status, when it get changes
        gameStatus.Subscribe(status =>
            {
                print("Game status: " + status.ToString());
                if (status == GameStatus.Completed)
                {
                    print("Finished");
                    btnRestart.gameObject.SetActive(true);

                    //if u enable the code, It will delete all cells which are resolved by the Context
                    //This also will delete all associated Views with the data cell objects.
//                    Context.DeleteAll<CellData>();
                }
            })
            .AddTo(gameObject);

        //setup button restart
        if (btnRestart)
        {
            btnRestart.onClick.AddListener(() => { StartCoroutine(RestartRoutine()); });
        }

        //setup the layout
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gameSetting.Width;
        
        //setup a new game
        Setup();
    }

    private void Setup()
    {
        btnRestart.gameObject.SetActive(false);
        
        //build the board
        gameBoard.Build(gameSetting.Width, gameSetting.Height, gameSetting.MineCount);

        print("Map setup");

        //solve the game
        StartCoroutine(SolveRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        yield return null;

        Context.DeleteAll<CellData>();

        gameStatus.Value = GameStatus.InProgress;
        
        Setup();
        
        //now scene loading is complete
    }

    IEnumerator SolveRoutine()
    {
        yield return gameSolver.Solve(1f);
    }
}