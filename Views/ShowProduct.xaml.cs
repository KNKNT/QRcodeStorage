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
        DataView dataView = new();              

        public ShowProduct()
        {
            InitializeComponent();
            LoadCategoriesComboBox();
            LoadTable();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e) => LoadTable();
        private void cbQrCode_SelectionChanged(object sender, SelectionChangedEventArgs e) => Search();
        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e) => Search();
        private void tbSearchName_TextChanged(object sender, TextChangedEventArgs e) => Search();
        private void btnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            tbSearchName.Clear();
            cbCategory.SelectedIndex = 0;   
            cbQrCode.SelectedIndex = 0;
        }

        private void LoadTable()
        {
            dataView = showProductModel.LoadProduct();
            dgProducts.ItemsSource = dataView;
            tblRowCount.Text = dataView.Count.ToString();
        }

        private void LoadCategoriesComboBox()
        {
            var categories = showProductModel.LoadCategories().Select(c => (c.Id, c.Category)).ToList();
            catigories.LoadComboBoxes(cbCategory, categories, "Все категории");
        }

        private void Search()
        {
            try
            {
                List<string> filters = new List<string>();

                if (!string.IsNullOrWhiteSpace(tbSearchName.Text))
                {
                    filters.Add($"[Name] LIKE '%{tbSearchName.Text}%'");
                }

                if (cbCategory.SelectedIndex > 0)
                {
                    string selectedCategory = cbCategory.Text.ToString();
                    filters.Add($"[Category] = '{selectedCategory}'");
                }

                if (cbQrCode.SelectedIndex > 0)
                {
                    bool hasQrCode = cbQrCode.SelectedIndex switch
                    {
                        1 => true,
                        2 => false
                    };
                    filters.Add($"[Qr] = {hasQrCode}");
                }

                string finalFilter = string.Join(" AND ", filters);
                dataView.RowFilter = finalFilter;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при фильтрации: {ex.Message}");
            }
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

    public class NullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value = string.IsNullOrEmpty(value.ToString()) ? "–" : value;
            return value;
        }

        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            value = value.ToString() == "–" ? null : value;
            return value;
        }
    }
}
