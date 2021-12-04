using Cinema.Desktop.Model;
using Cinema.Desktop.View;
using Cinema.Desktop.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Cinema.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private CinemaApiService _service;
        private MainViewModel _mainViewModel;
        private MainWindow _mainView;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MovieEditorWindow _editorView;
        private AddScreeningWindow _addScreeningView;
        private ShowScreeningWindow _showScreeningWindow;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _service = new CinemaApiService(ConfigurationManager.AppSettings["baseAddress"]);

            _loginViewModel = new LoginViewModel(_service);
            _loginViewModel.LoginSucceeded += _loginViewModel_LoginSucceeded;
            _loginViewModel.LoginFailed += _loginViewModel_LoginFailed;
            _loginViewModel.MessageApplication += onMessageApplication;

            _loginView = new LoginWindow()
            {
                DataContext = _loginViewModel
            };


            _mainViewModel = new MainViewModel(_service);
            _mainViewModel.MessageApplication += onMessageApplication;
            _mainViewModel.LogoutSucceeded += _mainViewModel_LogoutSucceeded;
            _mainViewModel.StartingMovieEdit += _mainViewModel_StartingItemEdit;
            _mainViewModel.FinishingMovieEdit += _mainViewModel_FinishingItemEdit;
            _mainViewModel.StartingImageChange += _mainViewModel_StartingImageChange;
            _mainViewModel.RefreshLabel += _mainViewModel_RefreshLabel;
            

            _mainView = new MainWindow
            {
                DataContext = _mainViewModel
            };
            _mainView.addScreeningButton.Click += AddScreeningButton_Click;
            _mainView.showScreeningButton.Click += ShowScreeningButton_Click;

            _addScreeningView = new AddScreeningWindow();
            _addScreeningView.addButton.Click += AddButton_Click;

            _loginView.Show();
        }

        private void _mainViewModel_RefreshLabel(object sender, EventArgs e)
        {
            _showScreeningWindow.showOrderLabel.Content = "ROW: " + _mainViewModel.BuyerRowID + " COLUMN: " + _mainViewModel.BuyerColumnID + " NAME: " + _mainViewModel.BuyerName + " PHONE: " + _mainViewModel.BuyerPhone;
        }

        private void ShowScreeningButton_Click(object sender, RoutedEventArgs e)
        {
            _showScreeningWindow = new ShowScreeningWindow();
            _showScreeningWindow.DataContext = _mainViewModel;
            _mainViewModel.LoadScreeningsAsync(_mainViewModel.SelectedItem);
            _showScreeningWindow.showSeatButton.Click += ShowSeatButton_Click;
            _showScreeningWindow.makePurchaseButton.Click += MakePurchaseButton_Click;
            _showScreeningWindow.showOrderLabel.Content = "ROW: " + _mainViewModel.BuyerRowID + " COLUMN: " + _mainViewModel.BuyerColumnID + " NAME: " + _mainViewModel.BuyerName + " PHONE: " + _mainViewModel.BuyerPhone; 
            _showScreeningWindow.ShowDialog();
        }

        private void MakePurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.PurchaseButtonCliked();
            
        }

        private void ShowSeatButton_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel.LoadSeatsAsync(_mainViewModel.SelectedScreening);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string movieName = (_addScreeningView.movieNameTextBox.Text).ToString();

            string screenTime = (_addScreeningView.screeningTimeTextBox.Text).ToString();

            String hallName = (_addScreeningView.screeningHallID.Text).ToString();

            _mainViewModel.AddScreening(movieName,screenTime, hallName);

            _addScreeningView.Close();
        }

        private void AddScreeningButton_Click(object sender, RoutedEventArgs e)
        {
            _addScreeningView.Show();
        }

        private void _mainViewModel_StartingItemEdit(object sender, EventArgs e)
        {
            _editorView = new MovieEditorWindow
            {
                DataContext = _mainViewModel
            };
            _editorView.ShowDialog();
        }

        private void _mainViewModel_FinishingItemEdit(object sender, EventArgs e)
        {
            if (_editorView.IsActive)
            {
                _editorView.Close();
            }
        }

        private async void _mainViewModel_StartingImageChange(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "Images|*.jpg;*.jpeg;*.bmp;*.tif;*.gif;*.png;",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog(_editorView).GetValueOrDefault(false))
            {
                _mainViewModel.EditableMovie.Image = await File.ReadAllBytesAsync(dialog.FileName);
            }
        }


        private void _mainViewModel_LogoutSucceeded(object sender, EventArgs e)
        {
            _mainView.Hide();
            _loginView.Show();
        }

        private void _loginViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("Login failed", "Cinema", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void _loginViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            _loginView.Hide();
            _mainView.Show();
        }

        private void onMessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Cinema", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
