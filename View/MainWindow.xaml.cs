using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Domain;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnMasterDataFilePathSelect_Click(object sender, RoutedEventArgs e)
        {
            TxtBoxFilePathMasterData.Text = _mainWindowViewModel.ChooseCSVFile();
        }

        private void BtnRouteNumberFilePathSelect_Click(object sender, RoutedEventArgs e)
        {
            TxtBoxFilePathRouteNumberOffer.Text = _mainWindowViewModel.ChooseCSVFile();
        }

        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            if (TxtBoxFilePathMasterData.Text.Equals("") || TxtBoxFilePathRouteNumberOffer.Text.Equals(""))
            {
                MessageBox.Show("Vælg venligst begge filer inden import startes");
            }
            else if (TxtBoxFilePathMasterData.Text.Equals("") && TxtBoxFilePathRouteNumberOffer.Text.Equals(""))
            {
                MessageBox.Show("Vælg venligst filerne inden import startes");
            }
            else
            {
                _mainWindowViewModel.ImportCSV(
                    TxtBoxFilePathMasterData.Text,
                    TxtBoxFilePathRouteNumberOffer.Text,
                    TxtBoxFilePathRouteNumbers.Text
                );
                MessageBox.Show("Filerne er nu importeret");
            }
        }

        private void BtnStartSelection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Offer> outputListByUserId = _mainWindowViewModel.InitializeSelection();
                ListView.ItemsSource = outputListByUserId;
                MessageBox.Show("Udvælgelsen er nu færdig");
            }
            catch (Exception x)
            {
                PromptWindow promptWindow = new PromptWindow(x.Message);
                promptWindow.Show();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Environment.Exit(1);
        }

        private void BtnSavePublic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _mainWindowViewModel.SaveCSVPublishFile();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }

        private void BtnSaveCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _mainWindowViewModel.SaveCSVCallFile();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
        }
    }
}