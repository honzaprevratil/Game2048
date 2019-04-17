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
        private string[] Colors = new string[] { "#F5F5F5", "#1565C0", "#558B2F", "#c62828", "#311B92", "#3E2723", "#004D40", "#263238", "#212121" };

        public MainWindow()
        {
            InitializeComponent();
            GenerateGrid();
            Render();
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
            if (Class2048.GameBoard[col, row] == 0)
            {
                button.Content = ""; // CONTENT
                button.Background = (Brush)BC.ConvertFrom("#F5F5F5"); // BRUSH
            }
            else if (Class2048.GameBoard[col, row] <= 8)
            {
                button.Content = Class2048.GameBoard[col, row]; // CONTENT
                button.Background = (Brush)BC.ConvertFrom("#DADADA"); // BRUSH
            }
        }
    }
}
