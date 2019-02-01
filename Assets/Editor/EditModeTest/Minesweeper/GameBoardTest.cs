using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using NUnit.Framework;
using System.Linq;
using App.Scripts.Boards;
using NUnit.Framework.Internal;
using UniRx;
using UnityEngine;
using UnityIoC;
using Random = System.Random;

[TestFixture]
public class GameBoardTest
{
    const int width = 10;
    const int height = 10;
    const int mines = 10;

    private const int firstMoveX = 1;
    private const int firstMoveY = 1;

    private Context context;
    private GameBoard gameBoard;
    private List<CellData> cells;
    private Random random;

    [SetUp]
    public void CommonInstall()
    {
        context = new Context();

        //Arrange
        gameBoard = context.Resolve<GameBoard>();
        cells = context.Resolve<List<CellData>>();
        random = context.Resolve<Random>();
        //Act
        gameBoard.Build(width, height, mines);
    }

    [Test]
    public void Test_GameBoard_Creation()
    {
        gameBoard.Build(width, height, mines);
        gameBoard.FirstMove(1, 1, random);
        //Assert
        Assert.AreEqual(width * height, cells.Count); //total of cells
        Assert.AreEqual(mines, cells.Count(cell => cell.IsMine.Value)); //total of mines
    }

    [Test]
    public void Test_GameBoard_FirstMove()
    {
        gameBoard.FirstMove(firstMoveX, firstMoveY, random);
        //Assert
        Assert.IsFalse(gameBoard.GetCellAt(firstMoveX, firstMoveY).IsMine.Value); //first move shouldn't get mine
    }

    [Test]
    public void Test_GameBoard_AdjacentMines()
    {
        gameBoard.GetCellAt(1, 1).IsMine.Value = true;
        gameBoard.GetCellAt(2, 1).IsMine.Value = false;
        gameBoard.GetCellAt(3, 1).IsMine.Value = true;
        gameBoard.GetCellAt(1, 2).IsMine.Value = false;
        gameBoard.GetCellAt(3, 2).IsMine.Value = true;
        gameBoard.GetCellAt(1, 3).IsMine.Value = false;
        gameBoard.GetCellAt(2, 3).IsMine.Value = true;
        gameBoard.GetCellAt(3, 3).IsMine.Value = false; //there are 4 mines around (2,2)

        //Assert
        Assert.AreEqual(4, gameBoard.GetNeighbors(2, 2).Count(z => z.IsMine.Value));
    }

    [Test]
    public void Test_GameBoard_Solver()
    {
        var status = context.Resolve<ReactiveProperty<GameStatus>>();
        var solver = context.Resolve<GameSolver>();
        //Act
        Observable.FromCoroutine(_ => solver.Solve(0f)).Subscribe(_ =>
        {
            //Assert
            //game should finish
            Assert.AreEqual(true, status.Value != GameStatus.InProgress);

            if (status.Value == GameStatus.Completed) //if solver success
            {
                Assert.AreEqual(cells.Count(data => data.IsMine.Value),
                    cells.Count(data => data.IsFlagged.Value));
            }
            else //if solver failed
            {
                Assert.AreEqual(cells.Count(data => data.IsMine.Value),
                    cells.Count(data => data.IsFlagged.Value));
            }
        });
    }
}