using System.Windows;
using System.Windows.Input;
using QRcodeStorage.Pages;
using QRcodeStorage.Views;
using QRcodeStorage.Views.UserControls;

namespace QRcodeStorage
{
    public partial class MainWindow : Window
    {
        private ucCamera ucCamera;
        private Type currentPageType;

        public MainWindow()
        {
            InitializeComponent();
            NavigationFrame.Content = new RegistrationPage();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();
        private void Button_Click_2(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void ShowProducts_Checked(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(ShowProduct));
        }

        private void CreateProducts_Checked(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(CreateProduct));
        }

        private void GenerateQR_Checked(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(GenerateQR));
        }
        private void ScanQR_Checked(object sender, RoutedEventArgs e)
        {
            NavigateToPage(typeof(ScanQR));
        }
        private async void NavigateToPage(Type pageType)
        {
            if (currentPageType == typeof(ScanQR))
                await ucCamera.StopCameraAsync();

            if (pageType == typeof(ScanQR))
                NavigationFrame.Content = new ScanQR();
            else
                NavigationFrame.Content = Activator.CreateInstance(pageType);
        }

    }
}