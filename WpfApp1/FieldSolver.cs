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

        public Dictionary<(int, int), List<(int, int)>> TreesToCells = new Dictionary<(int, int), List<(int, int)>>();
        public Dictionary<(int, int), List<(int, int)>> CellsToTrees = new Dictionary<(int, int), List<(int, int)>>();
        public void FillGrass()
        {
            for (int i = 0; i < field.SizeX; i++)
            {
                for (int j = 0; j < field.SizeX; j++)
                {
                    if (field.Cells[i, j].IsEmpty() && field.IsNotNearTree(i, j))
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
                if (fieldElements[n,i] is Space && ((Space)fieldElements[n, i]).state is Empty)
                {
                    TurnToGrass(n,i);
                }
            }
        }

        private void CleanRow(int n)
        {
            for (int i = 0; i < field.SizeX; i++)
            {
                if (fieldElements[i, n] is Space && ((Space)fieldElements[i,n]).state is Empty)
                {
                    TurnToGrass(i,n);
                }
            }
        }
        public delegate void CellDelegate(int x, int y);
        public void FillDictionaries()
        {
            TreesToCells = new Dictionary<(int, int), List<(int, int)>>();
            CellsToTrees = new Dictionary<(int, int), List<(int, int)>>();
            for (int i = 0; i < field.SizeX; i++)
            {
                for (int j = 0; j < field.SizeY; j++)
                {
                    if (field.Cells[i, j].IsTree())
                    {
                        List<(int, int)> cellsAroundTree = new List<(int, int)>();
                        ForEachBorderingCell(i, j, (int x, int y) => 
                        {
                            if (!field.Cells[x, y].IsTree())
                            {
                                cellsAroundTree.Add((x, y));
                            }
                        });

                        TreesToCells[(i, j)] = cellsAroundTree;
                    }
                    else
                    {
                        List<(int,int)> treesAroundCell = new List<(int, int)>();
                        ForEachBorderingCell(i, j, (int x, int y) =>
                        {
                            if (field.Cells[x,y].IsTree())
                            {
                                treesAroundCell.Add((x, y));
                            }
                        });
                        if(treesAroundCell.Count>0)
                        CellsToTrees[(i, j)] = treesAroundCell;
                    }
                }
            }
        }
        

        public void FindOneCellTrees()
        {
            foreach((int,int) tree in TreesToCells.Keys)
            {
                if (TreesToCells[tree].Count==1)
                {
                    int x = TreesToCells[tree][0].Item1;
                    int y = TreesToCells[tree][0].Item2;
                    TurnToTentForTree(x, y,tree);
                }
            }
        }
        public void FindTwoCellTrees()
        {
            foreach ((int, int) tree in TreesToCells.Keys)
            {
                if (TreesToCells[tree].Count == 2)
                {
                    int x1 = TreesToCells[tree][0].Item1;
                    int x2 = TreesToCells[tree][1].Item1;
                    int y1 = TreesToCells[tree][0].Item2;
                    int y2 = TreesToCells[tree][1].Item2;
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
                    {
                        TurnToGrass(x1 + x2 - tree.Item1, y1 + y2 - tree.Item2);
                        CellDelegate del1 = (X, Y) => { if ((X, Y) != tree && fieldElements[X, Y] is Tree) RemoveCellTreeConnection((X, Y), (x1, y1)); };
                        CellDelegate del2 = (X, Y) => { if ((X, Y) != tree && fieldElements[X, Y] is Tree) RemoveCellTreeConnection((X, Y), (x2, y2)); };
                        ForEachBorderingCell(x1, y1, del1);
                        ForEachBorderingCell(x2, y2, del2);
                    }
                }
            }
        }
        public void FindThreeCellTrees()
        {
            foreach((int,int) tree in TreesToCells.Keys)
            {
                if (TreesToCells[tree].Count == 3)
                {
                    int x, y;
                    (x, y) = FindMiddleCell(TreesToCells[tree]);
                    ForEachBorderingCell(x, y,(int X,int Y) => 
                    {
                        if (fieldElements[X,Y] is Tree &&(X,Y)!=tree)
                        {
                            RemoveCellTreeConnection((X, Y), (x, y));
                        }
                    });
                }
            }
            (int,int) FindMiddleCell(List<(int,int)> cells)
            {
                if (SimilarCells(cells[0], cells[1]))
                    return cells[2];
                if (SimilarCells(cells[0], cells[2]))
                    return cells[1];
                return cells[0];

                bool SimilarCells((int,int) cell1, (int, int) cell2)
                {
                    if (cell1.Item1 == cell2.Item1 || cell1.Item2 == cell2.Item2) 
                        return true;
                    return false;
                }
            }
        }
        public void FillReadyLines()
        {
            for (int i = 0; i < field.SizeY; i++)
            {
                if (field.verticalNumbers[i] == PlacedTentsInLine(i))
                    CleanLine(i);
            }
            for (int i = 0; i < field.SizeX; i++)
            {
                if (field.horizontalNumbers[i] == PlacedTentsInColumn(i))
                    CleanRow(i);
            }
        }
        public int PlacedTentsInColumn(int n)
        {
            int count = 0;
            for(int i=0;i<field.SizeX;i++)
            {
                if (fieldElements[i,n] is Space)
                if (((Space)fieldElements[i, n]).state is Tent)
                    count++;
            }
            return count;
        }
        public int PlacedTentsInLine(int n)
        {
            int count = 0;
            for(int i=0;i<field.SizeY;i++)
            {
                if (fieldElements[n,i] is Space)
                if (((Space)fieldElements[n, i]).state is Tent)
                    count++;
            }
            return count;
        }
        public void OneLineCombinations()
        {
            int series, tents;
            //обходим все столбцы
            for (int i = 0; i < field.SizeX; i++)
            {
                (series, tents) = (0, 0);
                //обходим столбец
                for (int j = 0; j < field.SizeY; j++)
                    AggregateCombinationsInCell(i, j, ref tents, ref series);

                tents += series / 2 + series % 2;
                series = 0;
                if (tents == field.verticalNumbers[i])
                {
                    for (int j = 0; j < field.SizeY; j++)
                    {
                        if (FieldElemIsEmpty(i, j))
                            series++;
                        else
                        {
                            if (series == 1)
                                TurnToTent(i, j - 1);
                            else if (series % 2 == 1)
                            {
                                for (int k = 1; k <= series; k++)
                                {
                                    if (k % 2 == 1)
                                        TurnToTent(i, j - k);
                                }
                            }
                            series = 0;
                        }
                    }
                    if (series == 1)
                        TurnToTent(i, field.SizeY - 1);
                    else if (series % 2 == 1)
                    {
                        for (int k = 1; k <= series; k++)
                        {
                            if (k % 2 == 1)
                                TurnToTent(i, field.SizeY - k);
                        }
                    }
                }
                else if (tents == field.verticalNumbers[i] + 1)
                {
                    int prevSeries = 0;
                    for (int j = 0; j < field.SizeY; j++)
                    {
                        if (FieldElemIsEmpty(i, j))
                            series++;
                        else
                        {
                            if (prevSeries % 2 == 1 && series % 2 == 1)
                            {
                                TurnToGrass(i + 1, j - series - 1);
                                TurnToGrass(i - 1, j - series - 1);
                            }
                            prevSeries = series;
                            series = 0;
                        }
                    }
                }
            }
            //обходим все строки
            for (int i = 0; i < field.SizeY; i++)
            {
                (series, tents) = (0, 0);
                for (int j = 0; j < field.SizeX; j++)
                    AggregateCombinationsInCell(j, i, ref tents, ref series);
                tents += series / 2 + series % 2;
                series = 0;
                if (tents == field.horizontalNumbers[i])
                {
                    for (int j = 0; j < field.SizeX; j++)
                    {
                        if (FieldElemIsEmpty(j, i))
                            series++;
                        else
                        {
                            if (series == 1)
                                TurnToTent(j - 1, i);
                            else if (series % 2 == 1)
                            {
                                for (int k = 1; k <= series; k++)
                                {
                                    if (k % 2 == 1)
                                        TurnToTent(j - k, i);
                                }
                            }
                            series = 0;
                        }
                    }
                    if (series == 1)
                        TurnToTent(field.SizeX-1, i);
                    else if (series % 2 == 1)
                    {
                        for (int k = 1; k <= series; k++)
                        {
                            if (k % 2 == 1)
                                TurnToTent(field.SizeX - k, i);
                        }
                    }
                }
                else if (tents == field.horizontalNumbers[i] + 1)
                {
                    int prevSeries = 0;
                    for (int j = 0; j < field.SizeX; j++)
                    {
                        if (FieldElemIsEmpty(j, i))
                            series++;
                        else
                        {
                            if (prevSeries % 2 == 1 && series % 2 == 1)
                            {
                                TurnToGrass(j - series - 1, i + 1);
                                TurnToGrass(j - series - 1, i - 1);
                            }
                            prevSeries = series;
                            series = 0;
                        }
                    }
                }
            }
        }

        private bool FieldElemIsEmpty(int i, int j)
        {
            return fieldElements[i,j] is Space && ((Space)fieldElements[i,j]).state is Empty;
        }
        private void AggregateCombinationsInCell(int x, int y, ref int tents, ref int series)
        {
            if (fieldElements[x, y] is Space)
            {
                if (((Space)fieldElements[x, y]).state is Tent)
                {
                    tents++;
                    tents += series / 2 + series % 2;
                    series = 0;
                }
                else if (((Space)fieldElements[x, y]).state is Grass )
                {
                    tents += series / 2 + series % 2;
                    series = 0;
                }
                else if (((Space)fieldElements[x, y]).state is Empty)
                    series++;
            }
            else if (fieldElements[x, y] is Tree)
            {
                tents += series / 2 + series % 2;
                series = 0;
            }
        }
        public void TurnToGrass(int x,int y)
        {
            if (field.IsInField(x, y))
            {
                if (!(fieldElements[x, y] is Tree))
                {
                    Space elem = (Space)fieldElements[x, y];
                    while (!(elem.state is Grass))
                    {
                        elem.ChangeState();
                    }
                    RemoveCellFromDictionaries(x, y);
                }
            }
        }
        public void TurnToTent(int x, int y)
        {
            if (field.IsInField(x, y))
            {
                if (!(fieldElements[x, y] is Tree))
                {
                    Space elem = (Space)fieldElements[x, y];
                    while (!(elem.state is Tent))
                    {
                        elem.ChangeState();
                    }
                    ForEachSemiBorderingCell(x, y, (int X, int Y) =>
                    {
                        if (!(fieldElements[X,Y] is Tree))
                        {
                            TurnToGrass(X, Y);
                        }
                    });
                }
            }
        }
        public void TurnToTentForTree(int x,int y,(int,int) tree)
        {
            if (field.IsInField(x, y))
            {
                if (!(fieldElements[x, y] is Tree))
                {
                    Space elem = (Space)fieldElements[x, y];
                    while (!(elem.state is Tent))
                    {
                        elem.ChangeState();
                    }
                    ForEachSemiBorderingCell(x, y, (int X, int Y) =>
                    {
                        if (!(fieldElements[X, Y] is Tree))
                        {
                            TurnToGrass(X, Y);
                        }
                    });
                    RemoveCellFromDictionaries(x, y);
                    RemoveTreeFromDictionaries(tree.Item1, tree.Item2);
                }
            }
        }
        public void RemoveCellFromDictionaries(int x,int y)
        {
            if (CellsToTrees.ContainsKey((x, y)))
            {
                for (int i = 0; i < CellsToTrees[(x, y)].Count; i++)
                {
                    TreesToCells[CellsToTrees[(x, y)][i]].Remove((x, y));
                }
                CellsToTrees.Remove((x, y));

            }
        }
        public void RemoveTreeFromDictionaries(int x,int y)
        {
            if (TreesToCells.ContainsKey((x, y)))
            {
                for (int i = 0; i < TreesToCells[(x, y)].Count; i++)
                {
                    CellsToTrees[TreesToCells[(x, y)][i]].Remove((x, y));
                }
            }
        }
        public void RemoveCellTreeConnection((int, int) tree, (int, int) cell)
        {
            int x, y, X, Y;
            (x, y) = (cell.Item1, cell.Item2);
            (X, Y) = (tree.Item1, tree.Item2);
            TreesToCells[(X, Y)].Remove((x, y));
            CellsToTrees[(x, y)].Remove((X, Y));
            if (CellsToTrees[(x, y)].Count == 0)
            {
                CellsToTrees.Remove((x, y));
                TurnToGrass(x, y);
            }
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
        public void ForEachSemiBorderingCell(int x, int y, CellDelegate del)
        {
            if (field.IsInField(x - 1, y))
                del(x - 1, y);
            if (field.IsInField(x - 1, y + 1))
                del(x - 1, y + 1);
            if (field.IsInField(x, y + 1))
                del(x, y + 1);
            if (field.IsInField(x + 1, y + 1))
                del(x + 1, y + 1);
            if (field.IsInField(x + 1, y))
                del(x + 1, y);
            if (field.IsInField(x + 1, y - 1))
                del(x + 1, y - 1);
            if (field.IsInField(x, y - 1))
                del(x, y - 1);
            if (field.IsInField(x - 1, y - 1))
                del(x - 1, y - 1);
        }
    }
}