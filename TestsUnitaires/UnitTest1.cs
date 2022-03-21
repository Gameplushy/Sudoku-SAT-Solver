using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSATSolver;
using System;

namespace TestsUnitaires
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            GrilleSudoku gs = new GrilleSudoku(4);
            CollectionAssert.AreEquivalent(gs.PrimeFactors, new int[] { 4, 2 });

            Assert.ThrowsException<ArgumentException>(() => { gs = new GrilleSudoku(10); });
        }
    }
}
