using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuSATSolver
{
    class GrilleSudoku
    {
        private int taille;
        private int[,] contenu;
        private bool boitesLongues;
        private int[] primeFactors;

        public int Taille { get => taille; }
        public int[,] Contenu { get => contenu; set => contenu = value; }
        public bool BoitesLongues { get => boitesLongues; }
        public int[] PrimeFactors { get => primeFactors; }

        public GrilleSudoku(int taille,int[,] grid,bool boitesLongues)
        {
            this.taille = taille;
            contenu = grid;
            this.boitesLongues = boitesLongues;
            primeFactors = PrimeFactorsOf(taille);
        }

        public GrilleSudoku(int taille,int[,] grid) : this(taille,grid, false) { }

        public string ShowGrid()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    sb.Append(contenu[i, j] + " ");
                    if ((j + 1) % (boitesLongues ? primeFactors[0] : primeFactors[1]) == 0 && j + 1 != taille) sb.Append("| ");
                }
                sb.AppendLine("");
                if ((i + 1) % (boitesLongues ? primeFactors[1] : primeFactors[0]) == 0 && i + 1 != taille)
                {
                    for (int k = 0; k < taille; k++)
                    {
                        sb.Append("--");
                        if ((k + 1) % (boitesLongues ? primeFactors[0] : primeFactors[1]) == 0 && k + 1 != taille) sb.Append("+-");
                    }
                    sb.AppendLine("");
                }
            }
            return sb.ToString();
        }

        public static int[] PrimeFactorsOf(int i)
        {
            switch (i)
            {
                case 4:
                    return new int[] { 2, 2 };
                case 6:
                    return new int[] { 2, 3 };
                case 8:
                    return new int[] { 2, 4 };
                case 9:
                    return new int[] { 3, 3 };
                default:
                    throw new InvalidOperationException();
            }
        }
    }

}
