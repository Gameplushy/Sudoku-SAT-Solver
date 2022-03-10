using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSATSolver
{
    class SolveurSAT
    {
        private List<List<int>> formules;
        private readonly BoolTernaire[] variables;

        public SolveurSAT(GrilleSudoku gs)
        {
            formules = new List<List<int>>();
            variables = new BoolTernaire[(int)Math.Pow(gs.Taille,3)];
            //1
            for (int i = 0; i < gs.Taille; i++)
            {
                for (int j = 0; j < gs.Taille; j++)
                {
                    for (int k = 0; k < gs.Taille; k++)
                    {
                        List<int> tmp = new List<int>();
                        if (gs.Contenu[i, j] == k + 1)
                        {
                            tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                            formules.Add(tmp);
                        }
                        else if(gs.Contenu[i,j] != 0)
                        {
                            tmp.Add(-(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1));
                            formules.Add(tmp);
                        }
                    }
                }
            }
            //2
            for (int i = 0; i < gs.Taille; i++)
            {
                for (int j = 0; j < gs.Taille; j++)
                {
                    List<int> tmp = new List<int>();
                    for (int k = 0; k < gs.Taille; k++)
                    {
                        tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                    }
                    formules.Add(tmp);
                }
            }
            //3
            for (int i = 0; i < gs.Taille; i++)
            {
                for (int j = 0; j < gs.Taille; j++)
                {
                    for (int k = 0; k < gs.Taille; k++)
                    {
                        for (int n = k + 1; n < gs.Taille; n++)
                        {
                            List<int> tmp = new List<int>();
                            tmp.Add(-(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1));
                            tmp.Add(-(i * gs.Taille + j + n * gs.Taille * gs.Taille + 1));
                            formules.Add(tmp);
                        }
                    }
                }
            }
            //4
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int j = 0; j < gs.Taille; j++)
                {
                    List<int> tmp = new List<int>();
                    for (int i = 0; i < gs.Taille; i++)
                    {
                        tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                    }
                    formules.Add(tmp);
                }
            }
            //5
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int i = 0; i < gs.Taille; i++)
                {
                    List<int> tmp = new List<int>();
                    for (int j = 0; j < gs.Taille; j++)
                    {
                        tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                    }
                    formules.Add(tmp);
                }
            }
            //6
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int iBlock = 0; iBlock < gs.Taille; iBlock += gs.PrimeFactors[1])
                {
                    for (int jBlock = 0; jBlock < gs.Taille; jBlock += gs.PrimeFactors[0])
                    {
                        List<int> tmp = new List<int>();
                        for (int i = iBlock; i < iBlock + gs.PrimeFactors[1]; i++)
                        {
                            for (int j = jBlock; j < jBlock + gs.PrimeFactors[0]; j++)
                            {
                                tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                            }

                        }
                        formules.Add(tmp);
                    }
                }
            }
            foreach (List<int> l in formules) Console.WriteLine(String.Join(' ', l.ToArray()) + " 0");
        }

        public int[,] Solve()
        {
            BoolTernaire[] res = SolveNode(0, true, variables);
            if (res == null) res = SolveNode(0, false, variables);
            return res == null ? null : TranslateToGrid(res);
        }

        private int[,] TranslateToGrid(BoolTernaire[] bt)
        {
            int size = (int)Math.Cbrt(bt.Length);
            int[,] newGrid = new int[size,size];
            for (int i = 0; i < bt.Length; i++)
            {
                if (bt[i] == BoolTernaire.True) newGrid[(i / size) % size, i % size] = (i / (int)Math.Pow(size, 2)) + 1; 
            }
            return newGrid;
        }

        private BoolTernaire[] SolveNode(int varAChanger, bool valAChanger, BoolTernaire[] listeVariables)
        {
            //Console.WriteLine("-{0} {1}", varAChanger, valAChanger);
            BoolTernaire[] input = new BoolTernaire[listeVariables.Length]; 
            Array.Copy(listeVariables, input, listeVariables.Length);
            input[varAChanger] = valAChanger?BoolTernaire.True:BoolTernaire.False;
            if (formules.Any(f => f.All(x => input[Math.Abs(x) - 1] != BoolTernaire.Unknown && (input[Math.Abs(x) - 1] == BoolTernaire.False && x > 0 || input[Math.Abs(x) - 1] == BoolTernaire.True && x < 0))))
            {
                //Console.WriteLine("{0} {1} ECHEC",varAChanger,valAChanger);
                return null;
            }

            if (varAChanger + 1 == input.Length)
                return input;       
            BoolTernaire[] res = SolveNode(varAChanger+1, !valAChanger, input);
            if (res == null) return SolveNode(varAChanger+1, valAChanger, input);
            return res;
        }
    }
}
