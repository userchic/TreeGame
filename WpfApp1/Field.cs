using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Field
    {
        public bool[,] Trees;
        public bool[,] Tents;

        public int[] horizontalNumbers;
        public int[] verticalNumbers;

        public int SizeX = 10;
        public int SizeY = 10;
        public Field(int sizeX,int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
        }
        public Field()
        {

        }
    }
}
