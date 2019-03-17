using System.Collections;
using System.Collections.Generic;
using System.Linq;
using App.Scripts.Boards;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityIoC;


public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private Button btnRestart;
    [SerializeField] private RectTransform container;

    [Singleton] private GameSetting gameSetting;
    [Singleton] private IGameBoard gameBoard;
    [Singleton] private List<Cell> cells;
    [Singleton] private List<CellData> cellData;
    [Singleton] private GameSolver gameSolver;
    
    private ReactiveProperty<GameStatus> gameStatus = new ReactiveProperty<GameStatus>();

    private void Awake()
    {
        AssemblyContext.GetDefaultInstance(this);
    }

    public void Start()
    {
        //setup game status, when it get changes
        gameStatus.Subscribe(status =>
            {
                print("Game status: " + status.ToString());
                if (btnRestart)
                {
                    btnRestart.gameObject.SetActive(status != GameStatus.InProgress);
                }
            })
            .AddTo(gameObject);

        //setup button restart
        if (btnRestart)
        {
            btnRestart.gameObject.SetActive(false);
            btnRestart.OnClickAsObservable()
                .Subscribe(unit => { StartCoroutine(RestartRoutine()); })
                .AddTo(gameObject);
        }

        //setup the layout
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gameSetting.Width;

        //build the board
        gameBoard.Build();

        //create cells
        foreach (var data in cellData)
        {
            var cell = AssemblyContext.GetDefaultInstance().Resolve<Cell>();
            cell.SetParent(container);
            cell.SetCellData(data);
            cells.Add(cell);
        }

        //solve the game
        Observable.FromCoroutine(_ => gameSolver.Solve(1f)).Subscribe(_ => { print("Finished"); })
            .AddTo(this);
    }

    IEnumerator RestartRoutine()
    {
        AssemblyContext.DisposeDefaultInstance();
        yield return null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //restart the game
    }
}