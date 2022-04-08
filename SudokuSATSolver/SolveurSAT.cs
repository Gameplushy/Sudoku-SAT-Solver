using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSATSolver
{
    public class SolveurSAT
    {
        private List<List<int>> formules;
        private readonly BoolTernaire[] variables;

        public SolveurSAT(GrilleSudoku gs)
        {
            formules = new List<List<int>>();
            variables = new BoolTernaire[(int)Math.Pow(gs.Taille,3)];
            //1 Indices
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
                            formules.AddRange(GetBadValues(i, j, gs.Contenu[i, j], gs));
                        }
                        else if(gs.Contenu[i,j] != 0)
                        {
                            tmp.Add(-(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1));
                            formules.Add(tmp);
                        }
                    }
                }
            }
            //2 Au moins un chiffre par case
            for (int i = 0; i < gs.Taille; i++)
            {
                for (int j = 0; j < gs.Taille; j++)
                {
                    List<int> tmp = new List<int>();
                    for (int k = 0; k < gs.Taille; k++)
                        tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                    formules.Add(tmp);
                }
            }
            //3 Au plus un chiffre par case
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
            //4 Chaque chiffre par colonne
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int j = 0; j < gs.Taille; j++)
                {
                    List<int> tmp = new List<int>();
                    for (int i = 0; i < gs.Taille; i++)
                    {
                        tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                        for(int ni = i + 1; ni < gs.Taille; ni++)
                        {
                            List<int> ntmp = new List<int>();
                            ntmp.Add(-(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1));
                            ntmp.Add(-(ni * gs.Taille + j + k * gs.Taille * gs.Taille + 1));
                            formules.Add(ntmp);
                        }
                    }
                    formules.Add(tmp);
                }
            }
            //5 Chaque chiffre par ligne
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int i = 0; i < gs.Taille; i++)
                {
                    List<int> tmp = new List<int>();
                    for (int j = 0; j < gs.Taille; j++)
                    {
                        tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                        for (int nj = j + 1; nj < gs.Taille; nj++)
                        {
                            List<int> ntmp = new List<int>();
                            ntmp.Add(-(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1));
                            ntmp.Add(-(i * gs.Taille + nj + k * gs.Taille * gs.Taille + 1));
                            formules.Add(ntmp);
                        }
                    }
                    formules.Add(tmp);
                }
            }
            //6 Chaque chiffre par bloc
            for (int k = 0; k < gs.Taille; k++)
            {
                for (int iBlock = 0; iBlock < gs.Taille; iBlock += gs.PrimeFactors[1])
                {
                    for (int jBlock = 0; jBlock < gs.Taille; jBlock += gs.PrimeFactors[0])
                    {
                        List<int> tmp = new List<int>();
                        for (int i = iBlock; i < iBlock + gs.PrimeFactors[1]; i++)
                            for (int j = jBlock; j < jBlock + gs.PrimeFactors[0]; j++)
                                tmp.Add(i * gs.Taille + j + k * gs.Taille * gs.Taille + 1);
                        formules.Add(tmp);
                    }
                }
            }
            //foreach (List<int> l in formules) Console.WriteLine(String.Join(' ', l.ToArray()) + " 0");
            //Console.WriteLine(formules.Count);
        }
        private int[,] TranslateToGrid(BoolTernaire[] bt)
        {
            int size = (int)Math.Cbrt(bt.Length);
            int[,] newGrid = new int[size, size];
            for (int i = 0; i < bt.Length; i++)
                if (bt[i] == BoolTernaire.True)
                    newGrid[(i / size) % size, i % size] = (i / (int)Math.Pow(size, 2)) + 1;    
            return newGrid;
        }

        public int[,] Solve()
        {
            BoolTernaire[] res = SolveNode(0, true, variables);
            if (res == null) res = SolveNode(0, false, variables);
            return res == null ? null : TranslateToGrid(res);
        }

        private BoolTernaire[] SolveNode(int varAChanger, bool valAChanger, BoolTernaire[] listeVariables)
        {
            BoolTernaire[] input = new BoolTernaire[listeVariables.Length]; 
            Array.Copy(listeVariables, input, listeVariables.Length);
            input[varAChanger] = valAChanger?BoolTernaire.True:BoolTernaire.False;
            //Si une des clause n'est pas respectée
            if (formules.Any(f => f.All(x => input[Math.Abs(x) - 1] != BoolTernaire.Unknown && (input[Math.Abs(x) - 1] == BoolTernaire.False && x > 0 || input[Math.Abs(x) - 1] == BoolTernaire.True && x < 0))))
                return null;
            //Si toutes les variables ont une valeur
            if (varAChanger + 1 == input.Length)
                return input;
            BoolTernaire[] res = SolveNode(varAChanger+1, !valAChanger, input);
            if (res == null) return SolveNode(varAChanger+1, valAChanger, input);
            return res;
        }

        private List<List<int>> GetBadValues(int i,int j, int val, GrilleSudoku gs)
        {
            List<List<int>> res = new List<List<int>>();
            for(int railI = 0; railI < gs.Taille; railI++)
            {
                for (int railJ = 0; railJ< gs.Taille; railJ++)
                {
                    if ((railI == i && railJ != j) || (railI!=i && railJ==j)) {
                        List<int> tmp = new List<int>();
                        tmp.Add(-(railI * gs.Taille + railJ + (val - 1) * gs.Taille * gs.Taille + 1));
                        res.Add(tmp);
                    }
                }
            }
            return res;
        }
    }
}
