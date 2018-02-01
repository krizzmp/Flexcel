using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class SelctionTest
    {
        private readonly SelectionController selectionController = new SelectionController();
        private readonly TestDataContainer testData = new TestDataContainer();

        [TestMethod]
        public void TestMethod_NoInputData()
        {
            selectionController.SelectWinners();
        }

        [TestMethod]
        public void TestMethod_HappyPath()
        {
            testData.FillListContainer_HappyPath();
            selectionController.SelectWinners();
        }
    }
}