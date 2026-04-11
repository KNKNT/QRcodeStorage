using QRcodeStorage.Models;
using QRcodeStorage.Services;
using QRcodeStorage.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QRcodeStorage.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateProduct.xaml
    /// </summary>
    public partial class CreateProduct : Page
    {
        public CreateProduct()
        {
            InitializeComponent();
            rbCreateNewProduct.IsChecked = true;   
        }
        private void rbCreateNewProduct_Checked(object sender, RoutedEventArgs e) => NavigationFrame.Content = new CreateNewProductPage();
        private void RadioButton_Checked(object sender, RoutedEventArgs e) => NavigationFrame.Content = new AddProductsPage();
    }
}
