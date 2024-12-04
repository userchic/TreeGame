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
        public Field field;
        public void CreateRandomField(object sender,EventArgs e)
        {
            //обнуляем и устанавливаем размер грида
            ClearGrid();
            //инициализация массивxов
            InitializeField();
            SetGridSize(field.SizeX + 1, field.SizeY + 1);
            //в гриде устанавливаем деревья с помощью генератора случайнызх чисел
            RandomizeField(20);
            //размещение элементов на поле
            PlaceFieldElements();
            //расчет кол-ва палаток и отображение его
            InitializeNumbers();
            PlaceNumbers();
        }
        public void InitializeField()
        {
            field = new Field();
            field.Tents = new bool[field.SizeX, field.SizeY];
            field.Trees = new bool[field.SizeX, field.SizeY];
            field.horizontalNumbers = new int[field.SizeX];
            field.verticalNumbers = new int[field.SizeY];
        }
        private void PlaceNumbers()
        {
            for (int i = 0;i < field.verticalNumbers.Length;i++)
            {
                Label label=new Label();
                SetLabel(field.verticalNumbers, i, i, field.SizeY + 1, label);
            }
            for (int i = 0; i < field.horizontalNumbers.Length; i++)
            {
                Label label=new Label();
                SetLabel(field.horizontalNumbers, i, field.SizeX + 1, i, label);
            }
        }
        public void SetLabel(int[] mas,int i,int x,int y,Label label)
        {
            label.Content = mas[i];
            Grid.SetColumn(label, x);
            Grid.SetRow(label, y);
            grid.Children.Add(label);
        }

        private void InitializeNumbers()
        {
            for(int i=0;i< field.Trees.GetLength(0);i++)
            {
                for (int j = 0; j < field.Trees.GetLength(1); j++)
                {
                    if (field.Tents[i,j]==true)
                    {
                        field.horizontalNumbers[j]++;
                        field.verticalNumbers[i]++;
                    }
                }
            }
        }

        private void PlaceFieldElements()
        {
            for (int i = 0; i < field.Trees.GetLength(0); i++)
            {
                for (int j = 0; j < field.Trees.GetLength(1); j++)
                {
                    if (field.Trees[i, j] == true)
                    {
                        Tree tree = new Tree();
                        SetElem(tree, i, j);
                    }
                    else
                    {
                        Space space = new Space();
                        SetElem(space, i, j);
                    }
                }
            } 
        }
        public void SetElem(UIElement elem,int x,int y)
        {
            Grid.SetColumn(elem, x);
            Grid.SetRow(elem, y);
            grid.Children.Add(elem);
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
                    (x, y) = (rand.Next(10), rand.Next(10));
                while (field.Trees[x, y] == true || field.Tents[x,y]==true || !TreeIsPlaceable(x,y));
                field.Trees[x, y] = true;
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
            field.Tents[x, y] = true;
        }

        public bool IsAvailableForTent(int x, int y, int side)
        {
            //ищем палатки с нужной стороны
            ChooseSide(ref x, ref y, side);
            
            //      x-1    x    x+1   
            // y-1              
            //  y        (x,y)  <----дерево   
            // y+1                      мы ищем вокруг него палатки

            if (IsInField(x, y))
            {
                for (int i = x - 1; i < x + 2; i++)
                {
                    for (int j = y - 1; j < y + 2; j++)
                    {
                        bool flag = IsInField(i, j);
                        if ( flag) 
                        {
                            if (field.Tents[i, j] == true)
                                return false;
                        }
                    }
                }
            }
            else
                return false;
            return true;
        }
        //метод меняет x и у чтобы они соответствовали элементу находящемуся с нужной стороны
        public void ChooseSide(ref int x,ref int y,int side)
        {
            //      x-1    x    x+1
            // y-1         1    
            //  y    0   (x,y)   2
            // y+1         3       

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
        public bool IsInField(int x, int y) => (x >= 0 & y >= 0) && (x < field.SizeX & y < field.SizeY);
    }
}
