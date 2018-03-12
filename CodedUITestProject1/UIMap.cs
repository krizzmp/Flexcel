using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;

namespace CodedUITestProject1
{
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using System.Drawing;


    public partial class UIMap
    {
        public void EnterDataPaths(string routeNumbersFilePath, string masterPath, string offersPath)
        {
            #region Variable Declarations
            WpfButton uIImporterButton = this.UIFlexSorteringWindow.UIImporterButton;
            WinButton uIOKButton = this.UIOKWindow.UIOKButton;
            WpfButton uIStartUdvælgelseButton = this.UIFlexSorteringWindow.UIStartUdvælgelseButton;
            WpfEdit filePathRouteNumbers = this.UIFlexSorteringWindow.UITxtBoxFilePathRouteNEdit1;
            WpfEdit filePathMaster = this.UIFlexSorteringWindow.UITxtBoxFilePathMasterEdit;
            WpfEdit filePathOffers = this.UIFlexSorteringWindow.UITxtBoxFilePathRouteNEdit;
            #endregion

            // Launch '%USERPROFILE%\Documents\flextrafik\Fynbus\Flexcel_Fynbus\Kode\Flexcel\View\bin\Debug\View.exe'
            ApplicationUnderTest uIFlexSorteringWindow = ApplicationUnderTest.Launch(this.RecordedMethod2Params.UIFlexSorteringWindowExePath, this.RecordedMethod2Params.UIFlexSorteringWindowAlternateExePath);
            
            filePathRouteNumbers.Text = routeNumbersFilePath;
            filePathMaster.Text = masterPath;
            filePathOffers.Text = offersPath;

            // Click 'Importer' button
            Mouse.Click(uIImporterButton, new Point(85, 24));

            // Click 'OK' button
            Mouse.Click(uIOKButton, new Point(88, 29));

            // Click 'Start Udvælgelse' button
            Mouse.Click(uIStartUdvælgelseButton, new Point(73, 18));

            // Click 'OK' button
            Mouse.Click(uIOKButton, new Point(30, 23));
            //uIFlexSorteringWindow.WaitForControlNotExist();

        }
        public void EnterDataPaths2(string routeNumbersFilePath, string masterPath, string offersPath)
        {
            #region Variable Declarations
            WpfButton uIImporterButton = this.UIFlexSorteringWindow.UIImporterButton;
            WinButton uIOKButton = this.UIOKWindow.UIOKButton;
            WpfButton uIStartUdvælgelseButton = this.UIFlexSorteringWindow.UIStartUdvælgelseButton;
            WpfEdit filePathRouteNumbers = this.UIFlexSorteringWindow.UITxtBoxFilePathRouteNEdit1;
            WpfEdit filePathMaster = this.UIFlexSorteringWindow.UITxtBoxFilePathMasterEdit;
            WpfEdit filePathOffers = this.UIFlexSorteringWindow.UITxtBoxFilePathRouteNEdit;

            WpfText uIDereringenbudpågaranText = this.UIPrioriteringWindow.UIDereringenbudpågaranText;
            WpfTable uIListViewTable = this.UIPrioriteringWindow.UIListViewTable;
            WpfControl uINavnHeaderItem = this.UIPrioriteringWindow.UIListViewTable.UIItemHeader.UINavnHeaderItem;
            #endregion

            // Launch '%USERPROFILE%\Documents\flextrafik\Fynbus\Flexcel_Fynbus\Kode\Flexcel\View\bin\Debug\View.exe'
            ApplicationUnderTest uIFlexSorteringWindow = ApplicationUnderTest.Launch(this.RecordedMethod2Params.UIFlexSorteringWindowExePath, this.RecordedMethod2Params.UIFlexSorteringWindowAlternateExePath);

            filePathRouteNumbers.Text = routeNumbersFilePath;
            filePathMaster.Text = masterPath;
            filePathOffers.Text = offersPath;

            // Click 'Importer' button
            Mouse.Click(uIImporterButton, new Point(85, 24));

            // Click 'OK' button
            Mouse.Click(uIOKButton, new Point(88, 29));

            // Click 'Start Udvælgelse' button
            Mouse.Click(uIStartUdvælgelseButton, new Point(73, 18));

            // Click 'OK' button
            //Mouse.Click(uIOKButton, new Point(30, 23));
            //uIFlexSorteringWindow.WaitForControlNotExist();
            //Mouse.Click(uIDereringenbudpågaranText, new Point(312, 15));

            //// Click 'listView' table
            //Mouse.Click(uIListViewTable, new Point(314, 88));

            //// Double-Click 'listView' table
            //Mouse.DoubleClick(uIListViewTable, new Point(464, 249));

            //// Click 'Navn' HeaderItem
            //Mouse.Click(uINavnHeaderItem, new Point(105, 29));

        }
        /// <summary>
        /// Assert3BidsForSameRoute - Use 'AssertMethod2ExpectedValues' to pass parameters into this method.
        /// </summary>
        public void Assert3BidsForSameRoute()
        {
            #region Variable Declarations
            WpfCell type2Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type2Cell;
            WpfCell type3Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type3Cell;
            WpfCell type6Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type6Cell;
            WpfCell type7Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type7Cell;
            WpfCell type5Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type5Cell;
            #endregion

            // Verify that the 'Name' property of '3' cell equals '3'
            Assert.AreEqual("3", type2Cell.Name);

            // Verify that the 'Name' property of '5' cell equals '1'
            Assert.AreEqual("1", type3Cell.Name);

            // Verify that the 'Name' property of '0' cell equals '0'
            Assert.AreEqual("0", type5Cell.Name);

            // Verify that the 'Name' property of '0' cell equals '0'
            Assert.AreEqual("0", type6Cell.Name);

            // Verify that the 'Name' property of '10' cell equals '10'
            Assert.AreEqual("10", type7Cell.Name);
        }
        public void AssertContractor()
        {
            #region Variable Declarations
            WpfCell type2Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type2Cell;
            WpfCell type3Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type3Cell;
            WpfCell type6Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type6Cell;
            WpfCell type7Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type7Cell;
            WpfCell type5Cell = this.UIFlexSorteringWindow.UIListViewTable.UIItemDataItem.Type5Cell;
            #endregion

            // Verify that the 'Name' property of '3' cell equals '3'
            Assert.AreEqual("1", type2Cell.Name);

            // Verify that the 'Name' property of '5' cell equals '1'
            Assert.AreEqual("1", type3Cell.Name);

            // Verify that the 'Name' property of '0' cell equals '0'
            Assert.AreEqual("0", type5Cell.Name);

            // Verify that the 'Name' property of '0' cell equals '0'
            Assert.AreEqual("0", type6Cell.Name);

            // Verify that the 'Name' property of '10' cell equals '10'
            Assert.AreEqual("10", type7Cell.Name);
        }
    }
}
