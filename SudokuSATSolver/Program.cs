using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SudokuSATSolver
{
    class Program
    {

        static void Main(string[] args)
        {
            int taille; //= 6;
            bool boitesLongues;// = false;
            int[,] grid; /*= new int[9, 9] {
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9},
                {1,2,3,4,5,6,7,8,9}
            };
            new int[4, 4]
            {
                {0,2,0,4 },
                {3,0,1,0 },
                {2,1,0,3 },
                {4,0,2,1 }
            };
            new int[6, 6]
            {
                {1,2,3,4,5,6 },
                {1,2,3,4,5,6 },
                {1,2,3,4,5,6 },
                {1,2,3,4,5,6 },
                {1,2,3,4,5,6 },
                {1,2,3,4,5,6 }
            };*/
            GrilleSudoku gs;
            SolveurSAT s;
            do
            {
                int sizeChoice = SetGridSize();
                if (sizeChoice % 2 == 0) boitesLongues = true;
                else boitesLongues = false;
                switch (sizeChoice)
                {
                    case 1:
                        taille = 4;
                        break;
                    case 2:
                    case 3:
                        taille = 6;
                        break;
                    case 4:
                    case 5:
                        taille = 8;
                        break;
                    case 6:
                        taille = 9;
                        break;
                    default: throw new ArgumentException();
                }
                Console.Clear();
                grid = SetLines(taille);
                gs = new GrilleSudoku(taille, grid, boitesLongues);
                s = new SolveurSAT(gs);
                //Demander Confirmation
                Console.Clear();
                Console.WriteLine("Vous avez soumis :");
                Console.WriteLine(gs.ShowGrid());
                Console.WriteLine("Est-ce ce que vous voulez ? Tapez 'y' pour continuer.");
            }
            while (!Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase));
            Console.WriteLine("Recherche de solution en cours. Cela peut prendre du temps...");
            gs.Contenu = s.Solve();
            Console.WriteLine("Voici la solution :");
            if (gs.Contenu == null) Console.WriteLine("Pas de réponse!!!");
            else Console.WriteLine(gs.ShowGrid());
            Console.ReadLine();
        }
        
        public static int SetGridSize()
        {
            string prompts = 
            @"Sélectionnez une grille de Sudoku :
            1. 4x4
            2. 6x6(blocs longs)
            3. 6x6(blocs larges)
            4. 8x8(blocs longs)
            5. 8x8(blocs larges)
            6. 9x9";
            Console.WriteLine(prompts);
            int res;
            do
            {
                if (int.TryParse(Console.ReadLine(), out res) && res >= 1 && res <= 6)
                    return res;
                Console.WriteLine("Veuillez réessayer.");
            }
            while (true);
         }

        public static int[,] SetLines(int taille)
        {
            int[,] grid = new int[taille, taille];
            int i = 0;
            string pattern = @"^(\s*[0-" + taille + "]){" + taille + @"}\s*$";
            while (i < taille)
            {
                Console.Clear();
                Console.WriteLine("Insérez vos {0} nombres. ({1}/{2})", taille, i, taille);
                string input = Console.ReadLine();
                if (Regex.IsMatch(input, pattern))
                {
                    char[] numbers = input.Replace(" ", String.Empty).ToCharArray();
                    for (int j = 0; j < taille; j++) grid[i, j] = int.Parse(numbers[j].ToString());
                    i++;
                }
                else Console.WriteLine("erreur");
            }
            return grid;
        }
    }
}
