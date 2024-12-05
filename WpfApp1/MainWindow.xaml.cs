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
            ClearGrid();
            InitializeField();
            SetGridSize(field.SizeX + 1, field.SizeY + 1);
            PlaceFieldElements();
            PlaceNumbers();
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
                ClearGrid();
                SetGridSize(field.SizeX + 1, field.SizeY + 1);
                PlaceFieldElements();
                PlaceNumbers();
            }
        }
        //инициализация поля случайными значениями
        public void InitializeField()
        {
            field = gen.Generate();
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
        //обнуляем и устанавливаем размер грида
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
    }
}
