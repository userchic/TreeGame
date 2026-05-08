using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace WpfApp1
{
    public class FieldGenerator: INotifyPropertyChanged
    {
        Random rand = new Random();
        Field Field;

        private int tents;
        private int xSize;
        private int ySize;
        public int Tents { get
            {
                return tents;
            }

            set
            {
                tents = value;
                OnPropertyChanged("Tents");
            }
        }
        public int XAxis {
            get
            {
                return xSize;
            }

            set
            {
                xSize = value;
                OnPropertyChanged("XSize");
            }
        }
        public int YAxis {
            get
            {
                return ySize;
            }

            set
            {
                ySize = value;
                OnPropertyChanged("YSize");
            }
        }
        public Field Generate()
        {
            Field = new Field(XAxis,YAxis);
            RandomizeField(Tents);
            return Field;
        }
        private void RandomizeField(int trees)
        {
            for (int i = 0; i < trees; i++)
            {
                int x, y;
                do
                    (x, y) = (rand.Next(Field.SizeX), rand.Next(Field.SizeY));
                while ( !Field.Cells[x, y].IsEmpty() || !TreeIsPlaceable(x, y));
                Field.PlaceTree(x, y);
                PlaceTentAround(x, y);
            }
        }
        private void PlaceTentAround(int x, int y)
        {
            int side;
            do
            {
                side = rand.Next(4);

            } while (!IsAvailableForTent(x, y, side));
            ChooseSide(ref x, ref y, side);
            Field.PlaceTent(x, y);
        }

        //метод меняет x и у чтобы они соответствовали элементу находящемуся с нужной стороны
        public void ChooseSide(ref int x, ref int y, int side)
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
        private bool TreeIsPlaceable(int x, int y)
        {
            for (int i = 0; i < 4; i++)
                if (IsAvailableForTent(x, y, i)) return true;
            return false;
        }



        public bool IsAvailableForTent(int x, int y, int side)
        {
            //ищем палатки с нужной стороны
            ChooseSide(ref x, ref y, side);

            //      x-1    x    x+1   
            // y-1              
            //  y        (x,y)  <----дерево   
            // y+1                      мы ищем вокруг него палатки

            if (Field.IsInField(x, y))
            {
                for (int i = x - 1; i < x + 2; i++)
                {
                    for (int j = y - 1; j < y + 2; j++)
                    {
                        if (Field.IsInField(i, j))
                        {
                            if (Field.Cells[i, j].IsTent())
                                return false;
                        }
                    }
                }
                return true;
            }
            else
                return false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if(PropertyChanged!=null)
                PropertyChanged(this,new PropertyChangedEventArgs(prop));
        }
    }
}