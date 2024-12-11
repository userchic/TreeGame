using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using WpfApp1.Elements;
using WpfApp1.NormalElements;

namespace WpfApp1
{
    public class FieldSolver
    {
        public Field field;
        public UIElement[,] fieldElements;

        public Dictionary<(int, int), List<(int, int)>> TreesTents = new Dictionary<(int, int), List<(int, int)>>();
        public void FillGrass()
        {
            for (int i = 0; i < field.SizeX; i++)
            {
                for (int j = 0; j < field.SizeX; j++)
                {
                    if (field.IsNotNearTree(i, j) & field.Cells[i, j].IsEmpty())
                    {
                        TurnToGrass(i, j);
                    }
                }
            }
        }

        public void CleanZeroLines()
        {
            for (int i = 0;i < field.SizeX; i++)
            {
                if (field.horizontalNumbers[i] == 0)
                    CleanRow(i);
            }
            for (int i = 0; i < field.SizeY; i++)
            {
                if (field.verticalNumbers[i] == 0)
                    CleanLine(i);
            }
        }

        private void CleanLine(int n)
        {
            for (int i = 0; i < field.SizeY; i++)
            {
                if (fieldElements[n,i] is Space)
                {
                    TurnToGrass(n,i);
                }
            }
        }

        private void CleanRow(int n)
        {
            for (int i = 0; i < field.SizeX; i++)
            {
                if (fieldElements[i, n] is Space)
                {
                    TurnToGrass(i,n);
                }
            }
        }

        public void FindSymmetricTrees()
        {

            foreach ((int, int) tree in TreesTents.Keys)
            {
                if (TreesTents[tree].Count == 2)
                {
                    int x1 = TreesTents[tree][0].Item1;
                    int x2 = TreesTents[tree][1].Item1;
                    int y1 = TreesTents[tree][0].Item2;
                    int y2 = TreesTents[tree][1].Item2;
                    if (Math.Abs(x1 - x2) == 2)
                    {
                        TurnToGrass((x1 + x2) / 2, y1 + 1);
                        TurnToGrass((x1 + x2) / 2, y1 - 1);
                    }
                    else if (Math.Abs(y1 - y2) == 2)
                    {
                        TurnToGrass(x1 + 1, (y1 + y2) / 2);
                        TurnToGrass(x1 - 1, (y1 + y2) / 2);
                    }
                    else
                        TurnToGrass(x1 + x2 - tree.Item1, y1 + y2 - tree.Item2);
                }
            }
        }
        public void RemoveFromBorderingTrees(int x,int y)
        {
            if (TreesTents.ContainsKey((x - 1, y))) TreesTents[(x - 1, y)]?.Remove((x - 1, y));
            if (TreesTents.ContainsKey((x, y + 1))) TreesTents[(x, y + 1)]?.Remove((x, y + 1));
            if (TreesTents.ContainsKey((x + 1, y))) TreesTents[(x + 1, y)]?.Remove((x + 1, y));
            if (TreesTents.ContainsKey((x, y - 1))) TreesTents[(x, y - 1)]?.Remove((x, y - 1));
        }

        public void TurnToGrass(int x,int y)
        {
            if (field.IsInField(x, y))
            {
                if (!(fieldElements[x, y] is Tree))
                {
                    Space elem = (Space)fieldElements[x, y];
                    RemoveFromBorderingTrees(x, y);
                    while (!(elem.state is Grass))
                    {
                        elem.ChangeState();
                    }
                }
            }
        }
        public void TurnToTent(int x, int y)
        {
            if(field.IsInField(x,y))
            if (!(fieldElements[x, y] is Tree))
            {
                Space elem = (Space)fieldElements[x, y];
                while (!(elem.state is Tent))
                {
                    elem.ChangeState();
                }
            }
        }
    }
}