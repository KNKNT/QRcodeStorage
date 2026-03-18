using QRcodeStorage.Models;
using QRcodeStorage.Services;
using System;
using System.Collections.Generic;
using System.Data;
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
using QRcodeStorage.Models;
using QRcodeStorage.Services;
using QRcodeStorage.Views;
using System.Diagnostics;


namespace QRcodeStorage.Views
{
    /// <summary>
    /// Логика взаимодействия для ProductLog.xaml
    /// </summary>
    public partial class ProductLog : Page
    {
        Loader loader = new();
        DataView dataView = new();
        Categories categoryList = new();
        public ProductLog()
        {
            InitializeComponent();
            LoadDataTable();
            LoadTypesComboBox();
        }

        private void LoadTypesComboBox()
        {
            var categories = loader.LoadTypes().Select(c => (c.Id, c.Type)).ToList();
            categoryList.LoadComboBoxes(cbTypeOperation, categories, "Все движения");
        }

        private void LoadDataTable()
        {
            dataView = loader.LoadDataTable("SELECT * FROM showmovements");
            dgProducts.ItemsSource = dataView;
            tblRowCount.Text = dataView.Count.ToString();
        }

        private void Search()
        {
            List<string> filters = new();
            if (tbDateFrom.SelectedDate.HasValue)
            {
                tbDateTo.DisplayDateStart = tbDateFrom.SelectedDate.Value;
                DateTime fromDate = tbDateFrom.SelectedDate.Value.Date;
                filters.Add($"date >= #{fromDate:yyyy-MM-dd}#");
            }
            if (tbDateTo.SelectedDate.HasValue)
            {
                tbDateFrom.DisplayDateEnd = tbDateTo.SelectedDate.Value;
                DateTime fromDate = tbDateTo.SelectedDate.Value.Date;
                filters.Add($"date < #{fromDate.AddDays(1):yyyy-MM-dd}#");
            }
            string result = string.Join(" AND ", filters);
            dataView.RowFilter = result;
        }

        private void cbTypeOperation_DropDownClosed(object sender, EventArgs e) => Search();

        private void tbDateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => Search();

        private void tbDateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e) => Search();

        private void btnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            dataView.RowFilter = null;
            tbDateTo.DisplayDateStart = null;
            tbDateFrom.DisplayDateEnd= null;
            cbTypeOperation.SelectedIndex = 0;
            tbProduct.Text = string.Empty;
        }
    }
}
