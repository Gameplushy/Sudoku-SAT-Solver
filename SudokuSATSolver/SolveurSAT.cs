using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSATSolver
{
    class SolveurSAT
    {
        private List<List<int>> formules;
        private BoolTernaire[] variables;

        private GrilleSudoku gs;

        public SolveurSAT(GrilleSudoku gs)
        {
            this.gs = gs;
            formules = new List<List<int>>();
            variables = new BoolTernaire[(int)Math.Pow(gs.Taille,3)];
            //1
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int i = 0; i < gs.Taille; i++)
                {
                    for (int j = 0; j < gs.Taille; j++)
                    {
                        if (gs.Contenu[i, j] == k + 1)
                        {
                            List<int> tmp = new List<int>();
                            tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
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
            int[] pF = new int[2] ; Array.Copy(gs.PrimeFactors,pF,2);
            if (!gs.BoitesLongues) pF = pF.Reverse().ToArray();
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int iBlock = 0; iBlock < gs.Taille; iBlock += pF[1])
                {
                    for (int jBlock = 0; jBlock < gs.Taille; jBlock += pF[0])
                    {
                        List<int> tmp = new List<int>();
                        for (int i = iBlock; i < iBlock + pF[1]; i++)
                        {
                            for (int j = jBlock; j < jBlock + pF[0]; j++)
                            {
                                tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                            }

                        }
                        formules.Add(tmp);
                    }
                }
            }
            //foreach (List<int> l in formules) Console.WriteLine(String.Join(' ', l.ToArray()) + " 0");
        }

        public int[,] Solve()
        {
            BoolTernaire[] res = SolveNode(0, BoolTernaire.True, variables);
            if (res == null) res = SolveNode(0, BoolTernaire.False, variables);
            return res == null ? null : TranslateToGrid(res);
        }

        private int[,] TranslateToGrid(BoolTernaire[] bt)
        {
            int[,] newGrid = new int[gs.Taille, gs.Taille];
            for (int i = 0; i < bt.Length; i++)
            {
                if (bt[i] == BoolTernaire.True) newGrid[(i / gs.Taille) % gs.Taille, i % gs.Taille] = (i / (int)Math.Pow(gs.Taille, 2)) + 1; 
            }
            return newGrid;
        }

        private BoolTernaire[] SolveNode(int varAChanger, BoolTernaire valAChanger, BoolTernaire[] listeVariables)
        {
            //Console.WriteLine("-{0} {1}", varAChanger, valAChanger);
            BoolTernaire[] input = new BoolTernaire[listeVariables.Length]; 
            Array.Copy(listeVariables, input, listeVariables.Length);
            input[varAChanger] = valAChanger;
            if (formules.Any(f => f.All(x => input[Math.Abs(x) - 1] != BoolTernaire.Unknown && (input[Math.Abs(x) - 1] == BoolTernaire.False && x > 0 || input[Math.Abs(x) - 1] == BoolTernaire.True && x < 0))))
            {
                //Console.WriteLine("{0} {1} ECHEC",varAChanger,valAChanger);
                return null;
            }

            if (varAChanger + 1 == input.Length)
            {
                //Console.WriteLine("TROUVE");
                return input;
            }
            BoolTernaire[] res = SolveNode(varAChanger+1, BoolTernaire.True, input);
            if (res == null) return SolveNode(varAChanger+1, BoolTernaire.False, input);
            return res;
        }
    }
}
