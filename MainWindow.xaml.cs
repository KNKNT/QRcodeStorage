using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using QRcodeStorage.Views.UserControls;
using QRcodeStorage.Views;
<<<<<<< HEAD
using QRcodeStorage.Views.UserControls;
=======
using QRcodeStorage.Pages;
>>>>>>> lost-commit

namespace QRcodeStorage
{
    public partial class MainWindow : Window
    {
        private ucCamera ucCamera;
        private Type currentPageType;

        public MainWindow()
        {
            InitializeComponent();
<<<<<<< HEAD
            NavigationFrame.Content = new RegistrationPage();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
=======
            cNavigationPanel.Width = new GridLength(0);
            NavigationFrame.Content = new Registration(this);
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();
        private async void Window_Closed(object sender, EventArgs e) => await new ucCamera().StopCameraAsync();
>>>>>>> lost-commit
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
<<<<<<< HEAD

=======
>>>>>>> lost-commit
            if (pageType == typeof(ScanQR))
                NavigationFrame.Content = new ScanQR();
            else
                NavigationFrame.Content = Activator.CreateInstance(pageType);
        }

    }
}