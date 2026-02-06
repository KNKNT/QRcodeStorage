using QRcodeStorage.Models;
using QRcodeStorage.Services;
using System.Data;
using System.Windows;
using System.Windows.Controls;


namespace QRcodeStorage.Pages
{
    /// <summary>
    /// Логика взаимодействия для ShowProduct.xaml
    /// </summary>
    public partial class ShowProduct : Page
    {
        Categories catigories = new();
        DataView dataView = new();    
        Loader loader = new();

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
            dataView = loader.LoadDataTable("SELECT * FROM ShowProducts");
            dgProducts.ItemsSource = dataView;
            tblRowCount.Text = dataView.Count.ToString();
        }

        private void LoadCategoriesComboBox()
        {
            var categories = loader.LoadCategories().Select(c => (c.Id, c.Category)).ToList();
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
}
