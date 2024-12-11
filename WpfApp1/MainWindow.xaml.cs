using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using WpfApp1.NormalElements;

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
        public FieldSolver solver = new FieldSolver();
        public UIElement[,] fieldElements;
        public void CreateRandomField(object sender,EventArgs e)
        {
            ClearGrid();
            GenerateField();
            solver.TreesTents=GetTrees();
            ConnectSolver();
            SetGridSize(field.SizeX + 1, field.SizeY + 1);
            PlaceFieldElements();
            solver.fieldElements = fieldElements;
            PlaceNumbers();
        }

        private void ConnectSolver()
        {
            solver.field=field;
        }
        public delegate void CellDelegate(int x, int y);
        public Dictionary<(int, int), List<(int, int)>> GetTrees()
        {
            Dictionary<(int, int), List<(int, int)>> trees = new Dictionary<(int, int), List<(int, int)>>();
            for (int i = 0; i < field.SizeX; i++)
            {
                for (int j = 0; j < field.SizeY; j++)
                {
                    if (field.Cells[i, j].IsTree())
                    {
                        List<(int, int)> cellsAroundTree = new List<(int, int)>();
                        ForEachBorderingCell(i, j, (int x, int y) => { if (!field.Cells[x,y].IsTree())cellsAroundTree.Add((x, y)); });
                        trees[(i, j)] = cellsAroundTree;
                    }
                }
            }
            return trees;
        }
        public void ForEachBorderingCell(int x, int y, CellDelegate del)
        {
            if (field.IsInField(x - 1, y))
                del(x - 1, y);
            if (field.IsInField(x, y + 1))
                del(x, y + 1);
            if (field.IsInField(x + 1, y))
                del(x + 1, y);
            if (field.IsInField(x, y - 1))
                del(x, y - 1);
        }
        public void SaveField(object sender,EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream f = new FileStream(saveFileDialog.FileName,FileMode.Create))
                {
                    BinaryFormatter formatter= new BinaryFormatter();
                    formatter.Serialize(f, field);
                }
            }
        }
        public void OpenField(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                using (FileStream f = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    field= (Field)formatter.Deserialize(f);
                }
                solver.TreesTents = GetTrees();
                ClearGrid();
                SetGridSize(field.SizeX + 1, field.SizeY + 1);
                PlaceFieldElements();
                PlaceNumbers();
            }
        }
        public void FillGrass(object sender, EventArgs e)
        {
            solver.FillGrass();
        }
        public void FindZeroLines(object sender,EventArgs e)
        {
            solver.CleanZeroLines();
        }
        public void FindSymmetricTrees1(object sender,EventArgs e)
        {
            solver.FindSymmetricTrees();
        }

        //инициализация поля случайными значениями
        public void GenerateField()
        {
            field = gen.Generate();
            fieldElements = new UIElement[field.SizeX, field.SizeY];
        }
        //отображение кол-ва палаток 
        private void PlaceNumbers()
        {
            for (int i = 0;i < field.verticalNumbers.Length;i++)
            {
                Label label=new Label();
                label.Content = field.verticalNumbers[i];
                SetElem(label, i, field.SizeY + 1);
            }
            for (int i = 0; i < field.horizontalNumbers.Length; i++)
            {
                Label label=new Label();
                label.Content = field.horizontalNumbers[i];
                SetElem(label, field.SizeX + 1, i);
            }
        }
            //размещение элементов на поле
        private void PlaceFieldElements()
        {
            for (int i = 0; i < field.SizeX; i++)
            {
                for (int j = 0; j < field.SizeY; j++)
                {
                    if (field.Cells[i, j].IsTree())
                    {
                        Tree tree = new Tree();
                        SetElem(tree, i, j);
                        fieldElements[i, j] = tree;
                    }
                    else
                    {
                        Space space = new Space();
                        SetElem(space, i, j);
                        fieldElements[i, j] = space;
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
        //обнуляем и устанавливаем размер грида
        public void ClearGrid()
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
        }
        public void SetGridSize(int horizontalN,int verticalN )
        {
            fieldElements = new UIElement[field.SizeX, field.SizeY];
            for (int i = 0; i < horizontalN; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width=new GridLength (40)});
            for (int i = 0; i < verticalN; i++)
                grid.RowDefinitions.Add(new RowDefinition() { Height=new GridLength(40)});
        }
    }
}
