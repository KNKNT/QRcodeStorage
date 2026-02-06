using QRcodeStorage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QRcodeStorage.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShowProduct.xaml
    /// </summary>
    public partial class ShowProduct : Page
    {
        ShowProductModel showProductModel = new();
        Categories catigories = new();

        public ShowProduct()
        {
            InitializeComponent();
            LoadCategoriesComboBox();
            LoadTable();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e) => LoadTable();

        private void LoadTable()
        {
            DataView dataView = new();
            dataView = showProductModel.LoadProduct();
            dgProducts.ItemsSource = dataView;
            tblRowCount.Text = dataView.Count.ToString();
        }
        private void LoadCategoriesComboBox()
        {
            var categories = showProductModel.LoadCategories().Select(c => (c.Id, c.Category)).ToList();
            catigories.LoadComboBoxes(cbCategory, categories, "Все категории");
        }
    }

    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Convert.ToInt32(value);
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Convert.ToBoolean(value);
            return value;
        }
    }
}
