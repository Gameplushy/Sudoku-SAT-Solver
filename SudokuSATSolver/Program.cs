using System;
using System.Diagnostics;

namespace SudokuSATSolver
{
    class Program
    {

        static void Main(string[] args)
        {
            GrilleSudoku gs;        
            do
            {
                int sizeChoice = SetGridSize();
                gs = new GrilleSudoku(sizeChoice);

                Console.Clear();
                SetLines(gs);
                
                //Console.Clear();
                //Demander Confirmation
                Console.WriteLine("Vous avez soumis :");
                Console.WriteLine(gs.ShowGrid());
                Console.WriteLine("Est-ce ce que vous voulez ? Tapez 'y' pour continuer.");
            }
            while (!Console.ReadLine().Equals("y", StringComparison.InvariantCultureIgnoreCase));
            SolveurSAT s = new SolveurSAT(gs);
            Console.WriteLine("Recherche de solution en cours. Cela peut prendre du temps...");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            gs.Contenu = s.Solve();
            sw.Stop();
            Console.WriteLine("Trouvé en {0} secondes",(float)sw.ElapsedMilliseconds/1000);
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

        public static void SetLines(GrilleSudoku gs)
        {
            int i = 0;
            string pattern = @"^(\s*[0-" + gs.Taille + "]){" + gs.Taille + @"}\s*$";
            while (i < gs.Taille)
            {
                Console.WriteLine("Insérez vos {0} nombres. (ligne {1}/{2})", gs.Taille, i+1, gs.Taille);
                string input = Console.ReadLine();
                if (gs.ParseLine(input, i))
                {
                    i++;
                }
                else Console.WriteLine("erreur");
            }
        }
    }
}
