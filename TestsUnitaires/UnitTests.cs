using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSATSolver;
using System;

namespace TestsUnitaires
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestGrilleSudokuConstructeur()
        {
            GrilleSudoku gs = new GrilleSudoku(4);
            CollectionAssert.AreEquivalent(gs.PrimeFactors, new int[] { 4, 2 });

            Assert.ThrowsException<ArgumentException>(() => { gs = new GrilleSudoku(10); });
        }

        [TestMethod]
        public void TestParseLine()
        {
            GrilleSudoku gs = new GrilleSudoku(6);
            Assert.AreEqual(false, gs.ParseLine("1_2057* 2",0));
            Assert.AreEqual(true, gs.ParseLine("1 0 2 0 5  7   9  1 2", 0));
        }

        [TestMethod]
        public void TestShowGrid()
        {
            GrilleSudoku gs = new GrilleSudoku(1);
            gs.Contenu = new int[,] { { 1, 2, 3, 4 }, { 1, 2, 3, 4 }, { 1, 2, 3, 4 }, { 1, 2, 3, 4 } };
            Assert.AreEqual(gs.ShowGrid(), @"1 2 │ 3 4 
1 2 │ 3 4 
────┼─────
1 2 │ 3 4 
1 2 │ 3 4 
");
        }
        [TestMethod]
        public void TestSolver()
        {
            GrilleSudoku gs = new GrilleSudoku(1);
            gs.Contenu = new int[,] { { 1, 0, 3, 0 }, { 0, 2, 0, 4 }, { 2, 0, 0, 0 }, { 0, 4, 0, 0 } };
            SolveurSAT s = new SolveurSAT(gs);
            Assert.IsNull(s.Solve());
            gs = new GrilleSudoku(1);
            gs.Contenu = new int[,] { { 1, 0, 3, 0 }, { 0, 4, 0, 2 }, { 2, 0, 4, 0 }, { 0, 3, 0, 1 } };
            s = new SolveurSAT(gs);
            Assert.IsNotNull(s.Solve());
        }
    }
}
