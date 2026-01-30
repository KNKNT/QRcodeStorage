using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QRcodeStorage.Pages;

namespace QRcodeStorage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
             
        }

        private void ShowProducts_Checked(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Content = new ShowProduct();
        }

        private void CreateProducts_Checked(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Content = new CreateProduct();
        }

        private void GenerateQR_Checked(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Content = new GenerateQR();
        }
        private void ScanQR_Checked(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Content = new ScanQR();
        }
    }
}