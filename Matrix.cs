using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EGA
{
    public class Matrix
    {
        private int ROWS;
        private int COLS;
        private double[,] values;
        public Matrix(string fileName)
        {
            string[] allLines = File.ReadAllLines(fileName);
            this.ROWS = allLines.Length;
            this.COLS = allLines.Length;
            values = new double[ROWS, COLS];
            for (int i=0;i<ROWS;i++)
            {
               string[] cols = allLines[i].Split(' ');
                for (int j = 0; j < COLS; j++)
                    values[i, j] = Convert.ToDouble(cols[j]);
            }

        }

        public void PrintMatrix()
        {
            for (int i=0; i<values.GetLength(0);i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                    System.Console.Write("{0,-6} ", values[i, j]);
                System.Console.WriteLine();
            }
        }

        public double this[int row, int col ]
        {
            get
            {
                return values[row, col];
            }
        }

        public int GetRange()
        {
            return COLS;
        }
    }
}
