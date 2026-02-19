using System;
using System.Collections.Generic;
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

namespace QRcodeStorage.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ucFileInfo.xaml
    /// </summary>
    public partial class ucFileInfo : UserControl
    {
        public ucFileInfo(bool isSuccess, string qrInfo, string path, string size)
        {
            InitializeComponent();

            tbFileSize.Text = size;
            tbPath.Text = path;
            tbQrInfo.Text = qrInfo;

            if (!isSuccess)
            {
                tbOperationType.Text = "Ошибка";
                brBack.BorderBrush = new SolidColorBrush(Color.FromRgb(244,67,54));
                icon.Fill = new SolidColorBrush(Color.FromRgb(244, 67, 54));
                brPath.BorderBrush = new SolidColorBrush(Color.FromRgb(244, 67, 54));
                brPath.Background = new SolidColorBrush(Color.FromRgb(53, 47, 56));
            }
        }
    }
}
