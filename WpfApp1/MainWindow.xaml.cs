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
        public bool [,] Trees;
        public bool [,] Tents;

        int[] horizontalNumbers;
        int[] verticalNumbers;

        public int fieldSizeX = 10;
        public int fieldSizeY = 10;
        public void CreateRandomField(object sender,EventArgs e)
        {
            //обнуляем и устанавливаем размер грида
            ClearGrid();
            SetGridSize(fieldSizeX + 1, fieldSizeY + 1);
            //в гриде устанавливаем деревья с помощью генератора случайнызх чисел
            Tents = new bool[fieldSizeX, fieldSizeY];
            Trees = new bool[fieldSizeX, fieldSizeY];
            horizontalNumbers = new int[fieldSizeX];
            verticalNumbers = new int[fieldSizeY];
            RandomizeField(10);
            PlaceFieldElements();
            InitializeNumbers();
            PlaceNumbers();
        }

        private void PlaceNumbers()
        {
            for (int i = 0;i < verticalNumbers.Length;i++)
            {
                Label label=new Label();
                label.Content = verticalNumbers[i];
                Grid.SetColumn(label, i);
                Grid.SetRow(label, fieldSizeY+1);
                grid.Children.Add(label);
            }
            for (int i = 0; i < horizontalNumbers.Length; i++)
            {
                Label label=new Label();
                label.Content = horizontalNumbers[i];
                Grid.SetRow(label, i);
                Grid.SetColumn(label, fieldSizeX + 1);
                grid.Children.Add(label);
            }
        }

        private void InitializeNumbers()
        {
            for(int i=0;i<Trees.GetLength(0);i++)
            {
                for (int j = 0; j < Trees.GetLength(1); j++)
                {
                    if (Tents[i,j]==true)
                    {
                        horizontalNumbers[j]++;
                        verticalNumbers[i]++;
                    }
                }
            }
        }

        private void PlaceFieldElements()
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
                    else
                    {
                        Space tree = new Space();
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
                } while (Trees[x, y] == true || Tents[x,y]==true || !TreeIsPlaceable(x,y));
                Trees[x, y] = true;
                PlaceTentAround(x, y);
            }
        }

        private bool TreeIsPlaceable(int x, int y)
        {
            for (int i = 0; i < 4; i++)
                if (IsAvailableForTent(x, y, i)) return true;
            return false;
        }

        private void PlaceTentAround(int x, int y)
        {
            int side;
            do
            {
                side = rand.Next(4);

            } while (!IsAvailableForTent(x, y, side));
            ChooseSide(ref x, ref y, side);
            Tents[x, y] = true;
        }

        public bool IsAvailableForTent(int x, int y, int side)
        {
            //      x-1    x    x+1   
            // y-1              
            //  y               
            // y+1               
            ChooseSide(ref x, ref y, side);
            if (IsInField(x, y))
            {
                for (int i = x - 1; i < x + 2; i++)
                {
                    for (int j = y - 1; j < y + 2; j++)
                    {
                        bool flag = IsInField(i, j);
                        if ((i != x || j != y) & flag) 
                        {
                            if (Tents[i, j] == true)
                                return false;
                        }
                    }
                }
            }
            else
                return false;
            return true;
        }
        public void ChooseSide(ref int x,ref int y,int side)
        {
            switch (side)
            {
                case 0:
                    x = x - 1; break;
                case 1:
                    y = y + 1; break;
                case 2:
                    x = x + 1; break;
                case 3:
                    y = y - 1; break;
            }
        }
        public bool IsInField(int x, int y) => (x >= 0 & y >= 0) && (x < fieldSizeX & y < fieldSizeY);
    }
}
