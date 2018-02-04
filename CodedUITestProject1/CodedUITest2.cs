using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace CodedUITestProject1
{
    /// <summary>
    /// Summary description for CodedUITest2
    /// </summary>
    [CodedUITest]
    public class CodedUITest2
    {
        public CodedUITest2()
        {
        }

        [TestMethod]
        public void CodedUiTest3BidsForSameRoute()
        {
            this.UIMap.EnterDataPaths(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Test med 3 bud til samme rute\RouteNumbers.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Test med 3 bud til samme rute\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Test med 3 bud til samme rute\Tilbud_FakeData.csv"
            );
            this.UIMap.Assert3BidsForSameRoute();
        }
        [TestMethod]
        public void CodedUiTestContractorNormal()
        {
            this.UIMap.EnterDataPaths(
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Contractor vinder ikke flere bud end han har biler til og vælger billigste l¢sning\RouteNumbers.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Contractor vinder ikke flere bud end han har biler til og vælger billigste l¢sning\Stamoplysninger_FakeData.csv",
                @"C:\Users\krizzmp\Documents\flextrafik\Fynbus\Flexcel_Fynbus\FakeData_Tests\Contractor vinder ikke flere bud end han har biler til og vælger billigste l¢sning\Tilbud_FakeData.csv"
            );
            this.UIMap.AssertContractor();
        }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
        //}

        #endregion

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private TestContext testContextInstance;

        public UIMap UIMap
        {
            get
            {
                if (this.map == null)
                {
                    this.map = new UIMap();
                }

                return this.map;
            }
        }

        private UIMap map;
    }
}