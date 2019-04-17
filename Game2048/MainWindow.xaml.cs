using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game2048
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int Cols = 4;
        public int Rows = 4;
        public int SizePx = 100;

        private Grid DynamicGrid { get; set; }
        public Class2048 Class2048 { get; set; }

        private BrushConverter BC = new BrushConverter();

        // Game Colors                           0,         2,         4,         8,         16,        32,        64,        128,       256,       512,       1024,      2048,      4096+
        private string[] Colors = new string[] { "#CDC1B3", "#EDE3D6", "#F0DFC1", "#F9B170", "#F49669", "#F3785F", "#F26048", "#EAD069", "#F0C962", "#EEC75F", "#F0C056", "#EABC4F", "#C9A142" };
        private int[] ColorIndexes = new int[] { 0, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048 };
        private string[] FontColors = new string[] { "#8E826C", "#FFFFFF" };

        private Key[] MoveKeys = new Key[] { Key.Down, Key.Up, Key.Left, Key.Right };

        public MainWindow()
        {
            InitializeComponent();
            GenerateGrid();
            Render();
        }

        private void Window_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Array.IndexOf(MoveKeys, e.Key) != -1)
            {
                GameState GameState = Class2048.Move(e.Key);
                Render();

                if (GameState == GameState.Lost)
                {
                    MessageBoxResult result = MessageBox.Show("You survived "+ Class2048.Turns+" turns and your final score is " + Class2048.GameScore + ".\nStart new Game?\n\nWARNING: No option exits the game!", "Game Over", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Class2048.NewGame();
                            Render();
                            break;
                        case MessageBoxResult.No:
                            Application.Current.Shutdown();
                            break;
                    }
                }

                if (GameState == GameState.Won)
                {
                    MessageBoxResult result = MessageBox.Show("You won! You made the 2048 in " + Class2048.Turns + " turns!\nContinue?\n\nWARNING: No option exits the game!", "You have won!", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Class2048.ContinueGameMode = true;
                            Render();
                            break;
                        case MessageBoxResult.No:
                            Application.Current.Shutdown();
                            break;
                    }
                }
            }
        }

        private void GenerateGrid()
        {
            DynamicGrid = new Grid
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5, 5, 5, 5),
                Width = (SizePx * Cols),
                Height = (SizePx * Rows),
            };
            Grid.SetRow(DynamicGrid, 1);
            Class2048 = new Class2048(Cols, Rows);

            // Create Columns
            for (int i = 0; i < Cols; i++)
            {
                ColumnDefinition gridCol = new ColumnDefinition
                {
                    Width = new GridLength(SizePx)
                };
                DynamicGrid.ColumnDefinitions.Add(gridCol);
            }

            // Create Rows
            for (int i = 0; i < Rows; i++)
            {
                RowDefinition gridRow = new RowDefinition
                {
                    Height = new GridLength(SizePx)
                };
                DynamicGrid.RowDefinitions.Add(gridRow);

            }

            int count = 1;

            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    Button MyControl1 = new Button
                    {
                        Name = "Button" + count.ToString(),
                        FontWeight = FontWeights.Bold,
                        FontSize = 32,
                        Content = ""
                    };

                    Grid.SetColumn(MyControl1, i);
                    Grid.SetRow(MyControl1, j);
                    DynamicGrid.Children.Add(MyControl1);

                    count++;
                }
            }

            if (MainGrid.Children.Count > 1)
            {
                MainGrid.Children.RemoveAt(1);
            }
            // Add the Grid as the Content of the Parent Window Object
            MainGrid.Children.Add(DynamicGrid);

            // bind time to display
            scoreLabel.SetBinding(ContentProperty, new Binding("GameScore") { Source = Class2048 });
            turnsLabel.SetBinding(ContentProperty, new Binding("Turns") { Source = Class2048 });

            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.Show();
        }

        private void Render()
        {
            foreach (Button child in DynamicGrid.Children)
            {
                int col = Grid.GetColumn(child);
                int row = Grid.GetRow(child);

                Set_Style(child, col, row);
            }
        }

        private void Set_Style(Button button, int col, int row)
        {
            // Background color
            // find index of color
            int indexOfColor = Array.IndexOf(ColorIndexes, Class2048.GameBoard[col, row]);
            if (indexOfColor == -1)
                indexOfColor = 12; // last index
            // paint background
            button.Background = (Brush)BC.ConvertFrom( Colors[indexOfColor] );

            // Content
            if (Class2048.GameBoard[col, row] == 0)
                button.Content = ""; // CONTENT
            else
                button.Content = Class2048.GameBoard[col, row]; // CONTENT

            // Foreground color = font Color
            if (Class2048.GameBoard[col, row] <= 4)
            {
                button.Foreground = (Brush)BC.ConvertFrom(FontColors[0]);
            }
            else
            {
                button.Foreground = (Brush)BC.ConvertFrom(FontColors[1]);
            }
        }
    }
}
