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
        public MainWindow()
        {
            InitializeComponent();
        }
        public Field field;
        public FieldGenerator gen=new FieldGenerator();
        public void CreateRandomField(object sender,EventArgs e)
        {
            //обнуляем и устанавливаем размер грида
            ClearGrid();
            //инициализация поля
            InitializeField();
            SetGridSize(field.SizeX + 1, field.SizeY + 1);
            //в гриде устанавливаем деревья с помощью генератора случайнызх чисел
            //размещение элементов на поле
            PlaceFieldElements();
            //расчет кол-ва палаток и отображение его
            PlaceNumbers();
        }
        public void InitializeField()
        {
            field = gen.Generate();
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
        private void PlaceFieldElements()
        {
            for (int i = 0; i < field.SizeY; i++)
            {
                for (int j = 0; j < field.SizeY; j++)
                {
                    if (field.Cells[i, j].IsTree())
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



        //метод меняет x и у чтобы они соответствовали элементу находящемуся с нужной стороны

    }
}
