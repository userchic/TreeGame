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

namespace WpfApp1.Elements
{
    /// <summary>
    /// Логика взаимодействия для Tree.xaml
    /// </summary>
    public partial class Tree : UserControl
    {
        public delegate void Handler();
        public event Handler Click;
        public Tree()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke();
        }
    }
}
