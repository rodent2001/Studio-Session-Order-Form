using StudioSessionOrderForm.ViewModels;
using System.Windows;

namespace StudioSessionOrderForm
{
    public partial class MainWindow : Window
    {
        // ViewModel
        private readonly MainWindowViewModel _viewModel;

        // Class constructor
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
        }

        // Buttons handlers
        private async void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.SaveOrderAsync();
        }
        private void ResetDataButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetOrderData();
        }
    }
}