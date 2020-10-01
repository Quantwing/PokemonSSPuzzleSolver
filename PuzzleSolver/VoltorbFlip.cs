using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace PuzzleSolver
{
    public class VoltorbFlip
    {
        public VoltorbFlip(PokemonSolvers form, int[] points, int[] orbs)
        {
            this.form = form;
            this.points = points;
            this.orbs = orbs;
        }

        public class Results
        {
            public Results(List<int[,]> cells, float timeTaken)
            {
                this.cells = cells;
                this.timeTaken = timeTaken;
            }

            public List<int[,]> cells;
            public float timeTaken;
        }

        private PokemonSolvers form;
        private List<int[,]> cells;
        private int currentResultIndex = 0;
        private readonly int[] cellOptions = new int[] { 0, 1, 2, 3 }; // 0 = orb, 1 - 3 = score
        private int[] points;
        private int[] orbs;

        public void Start()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            cells = new List<int[,]>();
            InitCellArray();
            Evaluate(0, 0);
            form.Invoke(new Action(() => form.VoltorbFlipSolverComplete(new Results(cells, stopwatch.ElapsedMilliseconds / 100))));
        }

        private void InitCellArray()
        {
            currentResultIndex = cells.Count;
            cells.Add(new int[points.Length / 2, points.Length / 2]);
            for (int i = 0; i < cells[currentResultIndex].GetLength(0); i++)
            {
                for (int p = 0; p < cells[currentResultIndex].GetLength(1); p++)
                {
                    if (currentResultIndex > 0)
                    {
                        cells[currentResultIndex][i, p] = cells[currentResultIndex - 1][i, p];
                    }
                    else
                    {
                        cells[currentResultIndex][i, p] = -1;
                    }
                }
            }
        }

        private void Evaluate(int widthIndex, int heightIndex)
        {
            for (int i = 0; i < cellOptions.Length; i++)
            {
                cells[currentResultIndex][widthIndex, heightIndex] = cellOptions[i];
                if (CheckValidCell(widthIndex, heightIndex))
                {
                    if (widthIndex == cells[currentResultIndex].GetLength(0) - 1 && heightIndex == cells[currentResultIndex].GetLength(1) - 1)
                    {
                        // Complete, start recursive return
                        InitCellArray();
                    }
                    else
                    {
                        Evaluate(widthIndex < cells[currentResultIndex].GetLength(0) - 1 ? widthIndex + 1 : 0, widthIndex < cells[currentResultIndex].GetLength(0) - 1 ? heightIndex : heightIndex + 1);
                    }
                }
            }
            cells[currentResultIndex][widthIndex, heightIndex] = -1;
        }

        private bool CheckValidCell(int widthIndex, int heightIndex)
        {
            int tempPoints = 0;
            int tempOrbs = 0;
            int empty = 0;

            // horizontal line points and orbs
            for (int i = 0; i < cells[currentResultIndex].GetLength(0); i++)
            {
                if (cells[currentResultIndex][i, heightIndex] == -1)
                {
                    empty++;
                }
                else if (cells[currentResultIndex][i, heightIndex] == 0)
                {
                    tempOrbs++;
                }
                else
                {
                    tempPoints += cells[currentResultIndex][i, heightIndex];
                }
            }

            if (tempPoints > points[heightIndex] || tempOrbs > orbs[heightIndex] || orbs[heightIndex] - tempOrbs + (int)Math.Ceiling((double)(points[heightIndex] - tempPoints) / cellOptions[cellOptions.Length - 1]) > empty)
            {
                return false;
            }

            tempPoints = 0;
            tempOrbs = 0;
            empty = 0;

            // vertical line points and orbs
            for (int i = 0; i < cells[currentResultIndex].GetLength(1); i++)
            {
                if (cells[currentResultIndex][widthIndex, i] == -1)
                {
                    empty++;
                }
                else if (cells[currentResultIndex][widthIndex, i] == 0)
                {
                    tempOrbs++;
                }
                else
                {
                    tempPoints += cells[currentResultIndex][widthIndex, i];
                }
            }

            if (tempPoints > points[cells[currentResultIndex].GetLength(0) + widthIndex] || tempOrbs > orbs[cells[currentResultIndex].GetLength(0) + widthIndex] || orbs[cells[currentResultIndex].GetLength(0) + widthIndex] - tempOrbs + (int)Math.Ceiling((double)(points[cells[currentResultIndex].GetLength(0) + widthIndex] - tempPoints) / cellOptions[cellOptions.Length - 1]) > empty)
            {
                return false;
            }

            return true;
        }
    }
}
