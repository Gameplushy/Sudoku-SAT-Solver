using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
        public int[] PrimeFactors { get => primeFactors; }

        public GrilleSudoku(int taille)
        {
            if (taille % 2 == 0) boitesLongues = true;
            else boitesLongues = false;
            switch (taille)
            {
                case 1:
                    this.taille = 4;
                    break;
                case 2:
                case 3:
                    this.taille = 6;
                    break;
                case 4:
                case 5:
                    this.taille = 8;
                    break;
                case 6:
                    this.taille = 9;
                    break;
                default: throw new ArgumentException();
            }
            contenu = new int[this.taille, this.taille];
            primeFactors = PrimeFactorsOf(this.taille);
        }

        public string ShowGrid()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < taille; i++)
            {
                for (int j = 0; j < taille; j++)
                {
                    sb.Append(contenu[i,j]==0?"_ ":contenu[i, j] + " ");
                    if ((j + 1) % primeFactors[0] == 0 && j + 1 != taille) sb.Append("│ ");
                }
                sb.AppendLine("");
                if ((i + 1) % primeFactors[1] == 0 && i + 1 != taille)
                {
                    for (int k = 0; k < taille; k++)
                    {
                        sb.Append("──");
                        if ((k + 1) % primeFactors[0] == 0 && k + 1 != taille) sb.Append("┼─");
                    }
                    sb.AppendLine("");
                }
            }
            return sb.ToString();
        }

        public int[] PrimeFactorsOf(int i)
        {
            int[] factors;
            switch (i)
            {
                case 4:
                    factors= new int[] { 2, 2 };
                    break;
                case 6:
                    factors= new int[] { 2, 3 };
                    break;
                case 8:
                    factors= new int[] { 2, 4 };
                    break;
                case 9:
                    factors= new int[] { 3, 3 };
                    break;
                default:
                    throw new InvalidOperationException();
            }
            if (!boitesLongues) return factors.Reverse().ToArray();
            return factors;
        }

        public bool ParseLine(string line,int lineNumber)
        {
            string pattern = @"^(\s*[0-" + taille + "]){" + taille + @"}\s*$";
            if (Regex.IsMatch(line, pattern))
            {
                char[] numbers = line.Replace(" ", String.Empty).ToCharArray();
                for (int j = 0; j < taille; j++) contenu[lineNumber, j] = int.Parse(numbers[j].ToString());
                return true;
            }
            return false;
        }
    }

}
