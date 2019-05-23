using System.Collections.Generic;
using System.Linq;
using Random = System.Random;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityIoC;

namespace App.Scripts.Boards
{
    public class GameBoard : IGameBoard
    {
        [Inject] IList<CellData> Cells;
//        ICollection<CellData> Cells = Pool<CellData>.List;
//        [Singleton] private List<CellData> Cells;
        [Singleton] private Observable<GameStatus> Status { get; set; }
        [Singleton] private GameSetting GameSettings { get; set; }

        public int Width
        {
            get { return GameSettings.Width; }
            set { GameSettings.Width = value; }
        }

        public int Height
        {
            get { return GameSettings.Height; }
            set { GameSettings.Height = value; }
        }

        public int MineCount
        {
            get { return GameSettings.MineCount; }
            set { GameSettings.MineCount = value; }
        }

        public GameBoard()
        {
        }

        /// <summary>
        /// Build a map using from setting file
        /// </summary>
        public void Build()
        {
            Debug.Assert(Cells != null);
            Build(GameSettings.Width, GameSettings.Height, GameSettings.MineCount);
        }

        public void Build(int width, int height, int mines)
        {
            var id = 0;
            for (var i = 1; i <= height; i++)
            {
                for (var j = 1; j <= width; j++)
                {
                    Context.Resolve<CellData>(id++, j, i);
                }
            }
        }

        public CellData GetCellAt(int x, int y)
        {
            return Cells.FirstOrDefault(z => z.X == x && z.Y == y);
        }

        /// <summary>
        /// Open a cell
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Open(int x, int y)
        {
            //Get the CellData
            var selectedCell = Cells.First(cell => cell.X == x && cell.Y == y);
            selectedCell.IsOpened.Value = true;

            //If open a mine, game over!
            if (selectedCell.IsMine.Value)
            {
                Status.Value = GameStatus.Failed;
            }

            //If the cell is a zero, open neighbors
            if (!selectedCell.IsMine.Value && selectedCell.AdjacentMines.Value == 0)
            {
                OpenEmptyCell(x, y);
            }

            //check if the game finished if there is no mine.
            if (!selectedCell.IsMine.Value)
            {
                CheckForCompletion();
            }
        }

        /// <summary>
        /// Open an empty cell
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void OpenEmptyCell(int x, int y)
        {
            var neighborCells = GetNeighbors(x, y).Where(panel => !panel.IsOpened.Value);
            foreach (var neighbor in neighborCells)
            {
                neighbor.IsOpened.Value = true;
                if (neighbor.AdjacentMines.Value == 0)
                {
                    OpenEmptyCell(neighbor.X, neighbor.Y);
                }

//                var cellData = neighbor;
//                Context.UpdateView(ref cellData);
            }
        }

        /// <summary>
        /// First opening of a cell of the game, then place mines
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="rand"></param>
        public void FirstMove(int x, int y, Random rand)
        {
            Debug.Log("First move, cell count: " + Cells.Count);

            var neighbors = GetNeighbors(x, y);
            neighbors.Add(GetCellAt(x, y)); //there is no mine around user's first move

            //Select other cells in the map
            var mineList = Cells.Except(neighbors).OrderBy(user => rand.Next());

            //select random cells to place mines
            var mineSlots = mineList.Take(MineCount).ToList().Select(z => new {z.X, z.Y});
            foreach (var mineCoord in mineSlots)
            {
                Cells.Single(cell => cell.X == mineCoord.X && cell.Y == mineCoord.Y).IsMine.Value = true;
            }

            //calculate how mant adjacent mines around a cell
            foreach (var openCell in Cells.Where(cell => !cell.IsMine.Value))
            {
                var nearbyCells = GetNeighbors(openCell.X, openCell.Y);
                openCell.AdjacentMines.Value = nearbyCells.Count(z => z.IsMine.Value);
            }
        }

        public List<CellData> GetNeighbors(int x, int y)
        {
            int depth = 1;

            var nearbyCells = Cells.Where(cell => cell.X >= (x - depth) && cell.X <= (x + depth)
                                                                        && cell.Y >= (y - depth) &&
                                                                        cell.Y <= (y + depth));
            var currentCell = Cells.Where(cell => cell.X == x && cell.Y == y);
            return nearbyCells.Except(currentCell).ToList();
        }


        private void CheckForCompletion()
        {
            var hiddenCells = Cells.Where(x => !x.IsOpened.Value).Select(x => x.ID);
            var mineCells = Cells.Where(x => x.IsMine.Value).Select(x => x.ID);
            if (!hiddenCells.Except(mineCells).Any())
            {
                Status.Value = GameStatus.Success;
            }
        }

        /// <summary>
        /// flag a cell
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Flag(int x, int y)
        {
            var cell = Cells.FirstOrDefault(z => z.X == x && z.Y == y);
            if (cell != null && !cell.IsOpened.Value)
            {
                cell.IsFlagged.Value = true;
            }
        }
    }
}