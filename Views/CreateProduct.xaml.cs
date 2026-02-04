using QRcodeStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
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
    /// Логика взаимодействия для CreateProduct.xaml
    /// </summary>
    public partial class CreateProduct : Page
    {
        CreateProductModel createProduct = new();
        Categories catigories = new();
        string? name, place, description;
        int? id, count, idCategory, idMaker;
        public CreateProduct()
        {
            InitializeComponent();
            LoadCategoriesComboBox();
            LoadMakersComboBox();
        }

        private void ClearData_Click(object sender, RoutedEventArgs e) => ClearData();
        private void LoadMakersComboBox()
        {
            var makers = createProduct.LoadMakers().Select(m => (m.Id, m.Maker)).ToList();
            catigories.LoadComboBoxes(cbMakers, makers);
        }
        private void LoadCategoriesComboBox()
        {
            var categories = createProduct.LoadCategories().Select(c => (c.Id, c.Category)).ToList();
            catigories.LoadComboBoxes(cbCategory, categories);
        }
        private void ClearData()
        {
            tbCount.Clear();
            tbDescription.Clear();
            tbPlace.Clear();
            tbProduct.Clear();

            cbCategory.Text = string.Empty;
            cbCategory.SelectedIndex = 0;
            cbMakers.Text = string.Empty;
            cbMakers.SelectedIndex = 0;
        }
        private void tbCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            bool isProductValid = !string.IsNullOrEmpty(tbProduct.Text);
            bool isCountValid = !string.IsNullOrEmpty(tbCount.Text);

            tblProductError.Visibility = isProductValid ? Visibility.Collapsed : Visibility.Visible;
            tblCountError.Visibility = isCountValid ? Visibility.Collapsed : Visibility.Visible;

            if (!isProductValid || !isCountValid)
                return;

            name = tbProduct.Text;
            count = Convert.ToInt32(tbCount.Text);

            try
            {
                description = string.IsNullOrEmpty(tbDescription.Text) ?
                    null : tbDescription.Text;

                place = string.IsNullOrEmpty(tbPlace.Text) ?
                    null : tbPlace.Text;

                idCategory = cbCategory.SelectedValue.Equals(0) ?
                    null : Convert.ToInt32(cbCategory.SelectedValue);

                idMaker = cbMakers.SelectedValue.Equals(0) ?
                    null : Convert.ToInt32(cbMakers.SelectedValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            Product product = new()
            {
                Name = name,
                Count = (int)count,
                IdCategory = idCategory,
                Place = place,
                IdMaker = idMaker,
                Description = description,
            };
            bool result = createProduct.InsertProduct(product);

            if (result)
                ClearData();
        }
    }
}
