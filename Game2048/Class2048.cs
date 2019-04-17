using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048
{
    public class Class2048 : INotifyPropertyChanged
    {
        private readonly Random RNG = new Random();

        private int Cols { get; set; }
        private int Rows { get; set; }
        public int[,] GameBoard { get; set; }

        public bool ContinueGameMode { get; set; } = false;

        List<int[]> FreeSpace { get; set; } = new List<int[]>();

        private int score = 0;
        public string GameScore
        {
            get
            {
                score = 0;
                for (int i = 0; i < Cols; i++)
                {
                    for (int j = 0; j < Rows; j++)
                    {
                        score += GameBoard[i, j];
                    }
                }
                return score.ToString();
            }
        }

        private int turns = 1;
        public int Turns
        {
            get
            {
                return turns;
            }
            set
            {
                turns = value;
                OnPropertyChanged("Turns");
            }
        }


        public Class2048(int cols, int rows)
        {
            Cols = cols;
            Rows = rows;
            GameBoard = new int[Cols, Rows];
            NewGame();
        }

        public void NewGame()
        {
            // fill with nulls
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    GameBoard[i, j] = 0;
                }
            }

            Turns = 1;
            ContinueGameMode = false;
            GenerateNewNumber();
            OnPropertyChanged("GameScore");
        }

        public GameState Move(Key pressedKey)
        {
            Direction direction;

            Borders borders;

            switch (pressedKey)
            {
                case Key.Down:
                    direction = new Direction(0, 1, Rows - 1);
                    borders = new Borders(Cols-1, -1, Rows-2, -1, -1);
                    break;
                case Key.Up:
                    direction = new Direction(0, -1, Rows - 1);
                    borders = new Borders(0, Cols, 1, Rows, +1);
                    break;
                case Key.Right:
                    direction = new Direction(1, 0, Cols - 1);
                    borders = new Borders(Cols - 2, -1, Rows-1, -1, -1);
                    break;
                case Key.Left:
                    direction = new Direction(-1, 0, Cols - 1);
                    borders = new Borders(1, Cols, 0, Rows, +1);
                    break;
                default:
                    return GameState.Lost;
            }

            MoveNumbers(direction, borders);
            MergeNumbers(direction, borders);
            MoveNumbers(direction, borders);

            GameState GameStatus = GenerateNewNumber();

            Turns = Turns + 1;
            OnPropertyChanged("GameScore");

            return GameStatus;
        }

        private void MoveNumbers(Direction direction, Borders borders)
        {
            for (int repeat = 1; repeat <= direction.MaxDistance; repeat++)
            {
                for (int x = borders.StartX; x != borders.EndX; x = x + borders.Change)
                {
                    for (int y = borders.StartY; y != borders.EndY; y = y + borders.Change)
                    {
                        // if this spot is not empty
                        if (GameBoard[x, y] != 0)
                        {
                            // if next spot is 0 (empty)
                            if (GameBoard[x + direction.X, y + direction.Y] == 0)
                            {
                                // move the number
                                GameBoard[x + direction.X, y + direction.Y] = GameBoard[x, y];
                                GameBoard[x, y] = 0;
                            }
                        }
                    }
                }
            }
        }

        private void MergeNumbers(Direction direction, Borders borders)
        {
            for (int x = borders.StartX; x != borders.EndX; x = x + borders.Change)
            {
                for (int y = borders.StartY; y != borders.EndY; y = y + borders.Change)
                {
                    // if this spot is not empty
                    if (GameBoard[x, y] != 0)
                    {
                        // if next spot is same as this spot
                        if (GameBoard[x + direction.X, y + direction.Y] == GameBoard[x, y])
                        {
                            // add the number
                            GameBoard[x + direction.X, y + direction.Y] += GameBoard[x, y];
                            GameBoard[x, y] = 0;
                        }
                    }
                }
            }
        }

        public GameState GenerateNewNumber()
        {
            FreeSpace.Clear();
            GameState OKGameStateReturn = GameState.Continue;

            // fill with nulls
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    if (GameBoard[i, j] == 0)
                        FreeSpace.Add(new int[2] { i, j });
                    if (GameBoard[i, j] == 2048)
                    {
                        if (!ContinueGameMode)
                        {
                            OKGameStateReturn = GameState.Won;
                        }
                    }
                }
            }

            if (FreeSpace.Count > 0)
            {
                // 80% chance to generate one number, 20% chance to generate two numbers
                int repeatNewNumber = 1;
                if (RNG.Next(1, 6) == 5)
                    repeatNewNumber = 2;

                for (int k = 1; k <= repeatNewNumber; k++)
                {
                    // 80% chance to 2, 20% chance to 4
                    int numberValue = 2;
                    if (RNG.Next(1, 6) == 5)
                        numberValue = 4;

                    // select random free spot
                    int[] selectedSpot = FreeSpace[RNG.Next(0, FreeSpace.Count)];
                    // place the number to the spot
                    GameBoard[selectedSpot[0], selectedSpot[1]] = numberValue;
                }

                return OKGameStateReturn;
            }
            else
            {
                return GameState.Lost;
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
