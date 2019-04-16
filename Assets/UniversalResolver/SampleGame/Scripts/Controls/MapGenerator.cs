/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Universal Resolver project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.Scripts.Boards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityIoC;


[ProcessingOrder(1)]
public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private Button btnRestart;
    [SerializeField] private RectTransform container;

    [Prefab] private Cell cell;
    [Singleton] private List<Cell> cells = new List<Cell>();
    [Singleton] private List<CellData> cellData = new List<CellData>();
    [Singleton] private IGameSolver gameSolver;
    [Singleton] private IGameBoard gameBoard;

    private GameSetting gameSetting = new GameSetting();
    private Observable<GameStatus> gameStatus = new Observable<GameStatus>();

    private void Awake()
    {
        if (!Context.Initialized)
        {
            Context.GetDefaultInstance(this);
        }

        //setup game status, when it get changes
        gameStatus.Subscribe(status => { print("Game status: " + status.ToString()); })
            .AddTo(gameObject);

        //setup button restart
        if (btnRestart)
        {
            btnRestart.gameObject.SetActive(false);
            btnRestart.onClick.RemoveAllListeners();
            btnRestart.onClick.AddListener(() =>
            {
                //todo: benmark load and reload scene
                //try to disable [Component] in Cell.cs
                
                Context.DefaultInstance.Dispose();
                Context.DefaultInstance = null;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart the game
            });
        }

        //setup the layout
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gameSetting.Width;

        //build the board
        gameBoard.Build();

        //create cells
        foreach (var data in cellData)
        {
//          var cellImg = Context.Instantiate(cell, container);
            var cellImg = Context.Resolve<Cell>(container);
            cellImg.SetCellData(data);
            cells.Add(cellImg);
        }

//        cell.gameObject.SetActive(false);
//        Destroy(cell.gameObject);

        print("Map setup");

        //solve the game
        StartCoroutine(SolveRoutine());
    }

    IEnumerator SolveRoutine()
    {
        yield return gameSolver.Solve(1f);

        print("Finished");
        btnRestart.gameObject.SetActive(true);
    }
}