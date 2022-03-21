using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSATSolver;
namespace SudokuSATSolverTest
{
    [TestClass]
    class UnitaryTests
    {
        [TestMethod]
        public void TestUn()
        {
            GrilleSudoku gs = new GrilleSudoku(4);
            Assert.Equals(gs.PrimeFactors, new int[] { 4, 2 });
        }
    }
}
