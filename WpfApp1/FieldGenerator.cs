using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace WpfApp1
{
    public class FieldGenerator
    {
        Random rand = new Random();
        Field Field;
        public Field Generate()
        {
            Field = new Field();
            RandomizeField(18);
            return Field;
        }
        private void RandomizeField(int trees)
        {
            for (int i = 0; i < trees; i++)
            {
                int x, y;
                do
                    (x, y) = (rand.Next(10), rand.Next(10));
                while (Field.Cells[x, y].IsTree() || Field.Cells[x, y].IsTent() || !TreeIsPlaceable(x, y));
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
            }
            else
                return false;
            return true;
        }
    }
}