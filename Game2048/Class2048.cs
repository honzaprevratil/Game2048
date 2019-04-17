using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048
{
    public class Class2048 : INotifyPropertyChanged
    {
        private Random RNG = new Random();

        private int Cols { get; set; }
        private int Rows { get; set; }
        public int[,] GameBoard { get; set; }

        private int score = 999;
        public string GameScore
        {
            get
            {
                return score.ToString();
            }
            set
            {
                score = int.Parse(value);
                OnPropertyChanged("GameScore");
            }
        }

        public Class2048(int cols, int rows)
        {
            Cols = cols;
            Rows = rows;
            GameBoard = new int[Cols, Rows];

            // fill with nulls
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    GameBoard[i, j] = 0;
                }
            }

            // static numbers
            GameBoard[0, 0] = 4;
            GameBoard[1, 3] = 2;
        }


        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
