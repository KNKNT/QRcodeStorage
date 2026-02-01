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
        DataBase dataBase = new();

        public CreateProduct()
        {
            InitializeComponent();
            LoadCategoriesComboBox();
            LoadMakersComboBox();
        }
        private void LoadMakersComboBox()
        {
            var makers = dataBase.LoadMakers().Select(m => (m.Id, m.Maker)).ToList();
            LoadComboBoxes(cbMakers, makers);
        }
        private void LoadCategoriesComboBox()
        {
            var categories = dataBase.LoadCategories().Select(c => (c.Id, c.Category)).ToList();
            LoadComboBoxes(cbCategory, categories);
        }

        private void LoadComboBoxes(ComboBox comboBox, List<(int Id, string Text)> data, string defaultText = "--Не выбрано--")
        {
            var items = new List<object>();
            items.Add(new { Id = 0, Text = defaultText });

            foreach (var item in data)
            {
                items.Add(new {item.Id, item.Text });
            }

            comboBox.ItemsSource = items;
            comboBox.DisplayMemberPath = "Text";
            comboBox.SelectedValuePath = "Id";
            comboBox.SelectedIndex = 0;
        }

        private void ClearData_Click(object sender, RoutedEventArgs e)
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
    }
}
