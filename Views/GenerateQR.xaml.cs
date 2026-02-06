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

namespace QRcodeStorage.Pages
{
    /// <summary>
    /// Логика взаимодействия для GenerateQR.xaml
    /// </summary>
    public partial class GenerateQR : Page
    {
        Loader loader = new();
        public GenerateQR()
        {
            InitializeComponent();
            dgProducts.ItemsSource = loader.LoadDataTable("SELECT name, qr FROM ShowProducts");
        }

    }
}
