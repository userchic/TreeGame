using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.FieldElements;

namespace WpfApp1
{
    public class Field
    {
        public Cell[,] Cells;

        public int[] horizontalNumbers;
        public int[] verticalNumbers;

        public int SizeX = 10;
        public int SizeY = 10;
        public Field(int sizeX,int sizeY)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            InitializeCells();
            horizontalNumbers = new int[SizeX];
            verticalNumbers = new int[SizeY];
        }
        public Field()
        {
            InitializeCells();
            horizontalNumbers = new int[SizeX];
            verticalNumbers = new int[SizeY];
        }
        public void InitializeCells()
        {
            Cells = new Cell[SizeX, SizeY];
            for (int i = 0; i < SizeX; i++)
                for (int j = 0; j < SizeY; j++)
                    Cells[i, j] = new Empty();
        }
        public void PlaceTent(int x,int y)
        {
            horizontalNumbers[y]++;
            verticalNumbers[x]++;
            Cells[x,y] = new Tent();
        }
        public void PlaceTree(int x, int y)
        {
            Cells[x, y] = new Tree();
        }
        public bool IsInField(int x, int y) => (x >= 0 & y >= 0) && (x < SizeX & y < SizeY);
    }
}
