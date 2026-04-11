using QRcodeStorage.Models;
using QRcodeStorage.Services;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using QRcodeStorage.Services;

namespace QRcodeStorage.Views
{
    /// <summary>
    /// Логика взаимодействия для AddProductsPage.xaml
    /// </summary>
    public partial class AddProductsPage : Page
    {
        Categories categories = new();
        DataView dataView = new();
        Loader loader = new();
        DataRowView row;

        private int count, currentCount, id;

        public AddProductsPage()
        {
            InitializeComponent();
            LoadTable();
            LoadCategoriesComboBox();
            LoadMakersComboBox();
        }
        
        private void LoadTable()
        {
            dataView = loader.LoadDataTable("SELECT * FROM ShowProducts");
            dgProducts.ItemsSource = dataView;
        }
        private void LoadCategoriesComboBox()
        {
            var _categories = loader.LoadCategories().Select(c => (c.Id, c.Category)).ToList();
            categories.LoadComboBoxes(cbCategory, _categories);
        }
        private void LoadMakersComboBox()
        {
            var makers = loader.LoadMakers().Select(m => (m.Id, m.Maker)).ToList();
            categories.LoadComboBoxes(cbMakers, makers);
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

                if (cbMakers.SelectedIndex > 0)
                {
                    string selectedCategory = cbMakers.Text.ToString();
                    filters.Add($"[Maker] = '{selectedCategory}'");
                }

                string finalFilter = string.Join(" AND ", filters);
                dataView.RowFilter = finalFilter;
            }
            catch (Exception ex)
            {
                Notification.Show(false, "Ошибка при фильтрации", ex.Message);
            }
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            row = (DataRowView)dgProducts.SelectedItem;
            currentCount = Convert.ToInt32(row["count"]);
            id = Convert.ToInt32(row["id_product"]);
            count = currentCount;
            tblCount.Text = count.ToString();
            Counter.IsEnabled = true;
            btnAddProduct.IsEnabled = true;
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if(count > 1)
            {
                count--;
                tblCount.Text = count.ToString();
            }
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            count++;
            tblCount.Text = count.ToString();
        }

        private void tbSearchName_TextChanged(object sender, TextChangedEventArgs e) => Search();
        private void cbCategory_DropDownClosed(object sender, EventArgs e) => Search();
        private void cbMakers_DropDownClosed(object sender, EventArgs e) => Search();

        private void tblCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(uint.TryParse(tblCount.Text, out uint newCount) & newCount != 0)
            {
                Counter.BorderBrush = Brushes.Gray;
                count = (int)newCount;
            }
            else
            {
                Notification.Show(false, "Ошибка", "Неверный формат");
                Counter.BorderBrush = Brushes.Red;
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            int totalCountOfProduct = count + currentCount;
            loader.AddCount(id, totalCountOfProduct);
            row["count"] = totalCountOfProduct;
        }
    }
}
