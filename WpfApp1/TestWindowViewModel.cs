using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class TestWindowViewModel:INotifyPropertyChanged
    {
        private FieldGenerator currentGenerator;
        private Field field;
        public Field Field { 
            get
            {
                return field;
            }
            set
            {
                field= value;
                OnPropertyChanged("Field");
            }
        }

        public FieldGenerator CurrentGenerator { 
            get 
            {
                return currentGenerator; 
            }
            set 
            { 
                currentGenerator = value;
                OnPropertyChanged("CurrentGenerator");
            }
        }
        public TestWindowViewModel()
        {
            currentGenerator = new FieldGenerator();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
