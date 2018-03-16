using System.Collections.Generic;
using System.Linq;
using Logic;
using Microsoft.Win32;
using System.Windows;
using System.Threading;
using Domain;

namespace View
{
    public class MainWindowViewModel
    {
        private readonly IoController _iOController = new IoController();
        private bool _selectionDone = false;
        private bool _importDone = false;

        public void ImportCSV(string masterDataFilepath, string routeNumberFilepath, string routeNumbersFilepath)
        {
            _iOController.InitializeImport(masterDataFilepath, routeNumberFilepath, routeNumbersFilepath);
            _importDone = true;
        }
        public string ChooseCSVFile()
        {
            string filename = "Ingen fil er valgt";
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "CVS filer (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
            }
            return filename;
        }

        public void SaveCSVCallFile()
        {
            if (_selectionDone == true)
            {
                SaveFileDialog saveDlg = new SaveFileDialog
                {
                    Filter = "CSV filer (*.csv)|*.csv|All files (*.*)|*.*",
                    InitialDirectory = @"C:\%USERNAME%\"
                };

                saveDlg.ShowDialog();

                string path = saveDlg.FileName;
                _iOController.InitializeExportToCallingList(path);
                MessageBox.Show("Filen er gemt.");
            }
            else
            {
                MessageBox.Show("Du har ikke udvalgt vinderne endnu.. Kør Udvælgelse først!");
            }
        }
        public void SaveCSVPublishFile()
        {
            if (_selectionDone == true)
            {
                SaveFileDialog saveDlg = new SaveFileDialog
                {
                    Filter = "CSV filer (*.csv)|*.csv|All files (*.*)|*.*",
                    InitialDirectory = @"C:\%USERNAME%\"
                };

                saveDlg.ShowDialog();

                string path = saveDlg.FileName;

                _iOController.InitializeExportToPublishList(path);
                MessageBox.Show("Filen er gemt.");
            }
            else
            {
                MessageBox.Show("Du har ikke udvalgt vinderne endnu.. Kør Udvælgelse først!");
            }
        }
        public List<Offer> InitializeSelection()
        {
            if (_importDone)
            {
                _selectionDone = true;
            }
            else
            {
                MessageBox.Show("Du skal importere filerne først.");
            }
            new SelectionController().Start();
            ListContainer listContainer = ListContainer.Instance;
            List<Offer> outputListByUserId = listContainer.OutputList;
            return outputListByUserId;
        }
    }
}
