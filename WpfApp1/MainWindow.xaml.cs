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
using WpfApp1.Elements;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random rand=new Random();
        public MainWindow()
        {
            InitializeComponent();
        }
        bool [,] Trees;
        int[] horizontalNumbers;
        int[] verticalNumbers;
        public void CreateRandomField(object sender,EventArgs e)
        {
            //обнуляем и устанавливаем размер грида
            ClearGrid();
            SetGridSize(11, 11);
            //в гриде устанавливаем деревья с помощью генератора случайнызх чисел
            Trees = new bool[10, 10];
            RandomizeField(30);
            PlaceFieldTrees();
            InitializeNumbers();
        }

        private void InitializeNumbers()
        {
            for(int i=0;i<Trees.GetLength(0);i++)
            {
                
            }
        }

        private void PlaceFieldTrees()
        {
            for (int i = 0; i < Trees.GetLength(0); i++)
            {
                for (int j = 0; j < Trees.GetLength(1); j++)
                {
                    if (Trees[i, j] == true)
                    {
                        Tree tree = new Tree();
                        Grid.SetColumn(tree, i);
                        Grid.SetRow(tree, j);
                        grid.Children.Add(tree);
                    }
                }
            } 
        }

        public void ClearGrid()
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
        }
        public void SetGridSize(int horizontalN,int verticalN )
        {
            for (int i = 0; i < horizontalN; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < verticalN; i++)
                grid.RowDefinitions.Add(new RowDefinition());
        }
        private void RandomizeField(int trees)
        {
            for (int i=0;i<trees; i++)
            {
                int x, y;
                do
                {
                    (x, y) = (rand.Next(10), rand.Next(10));
                } while (Trees[x, y] == true);
                Trees[x, y] = true;
            }
        }
    }
}
