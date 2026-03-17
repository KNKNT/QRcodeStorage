using QRcodeStorage.Entity;
using QRcodeStorage.Models;
using QRcodeStorage.Pages;
using QRcodeStorage.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для ucProductInfo.xaml
    /// </summary>
    public partial class ucProductInfo : UserControl
    {

        public static readonly DependencyProperty IdProperty =
         DependencyProperty.Register("Id", typeof(int), typeof(ucProductInfo),
             new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty ProductNameProperty =
            DependencyProperty.Register("ProductName", typeof(string), typeof(ucProductInfo),
                new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty CategoryProperty =
            DependencyProperty.Register("Category", typeof(string), typeof(ucProductInfo),
                new PropertyMetadata("-", OnPropertyChanged));

        public static readonly DependencyProperty MakerProperty =
            DependencyProperty.Register("Maker", typeof(string), typeof(ucProductInfo),
                new PropertyMetadata("-", OnPropertyChanged));

        public static readonly DependencyProperty PlaceProperty =
            DependencyProperty.Register("Place", typeof(string), typeof(ucProductInfo),
                new PropertyMetadata("-", OnPropertyChanged));

        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register("Count", typeof(int), typeof(ucProductInfo),
                new PropertyMetadata(0, OnPropertyChanged));

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ucProductInfo),
                new PropertyMetadata("-", OnPropertyChanged));

        public int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        public string ProductName
        {
            get { return (string)GetValue(ProductNameProperty); }
            set { SetValue(ProductNameProperty, value); }
        }

        public string Category
        {
            get { return (string)GetValue(CategoryProperty); }
            set { SetValue(CategoryProperty, value); }
        }

        public string Maker
        {
            get { return (string)GetValue(MakerProperty); }
            set { SetValue(MakerProperty, value); }
        }
        public string Place
        {
            get { return (string)GetValue(PlaceProperty); }
            set { SetValue(PlaceProperty, value); }
        }

        public int Count
        {
            get { return (int)GetValue(CountProperty); }
            set
            {
                SetValue(CountProperty, value);
                tbCount.Text = value.ToString();   
            }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        private int operationCount = 1;

        private int OperationCount
        {
            get => operationCount;
            set
            {
                operationCount = value;
                tblCount.Text = operationCount.ToString();
            }
        }

        User user = new();
        Loader loader = new();
        ScanQRModel scanQRModel = new ScanQRModel();


        public ucProductInfo()
        {
            InitializeComponent();
            DataContext = this;
            UpdateUI();
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ucProductInfo)d;

            control.OperationCount = 1;
            control.UpdateUI();
            control.UpdateButtonsState();
        }

        private void UpdateUI()
        {
            tblCount.Text = operationCount.ToString();
            tbId.Text = Id.ToString();
            tbProductName.Text = string.IsNullOrEmpty(ProductName) ? "-" : ProductName;
            tbCategory.Text = string.IsNullOrEmpty(Category) ? "-" : Category;
            tbMaker.Text = string.IsNullOrEmpty(Maker) ? "-" : Maker;
            tbCount.Text = Count.ToString();
            tbPlace.Text = string.IsNullOrEmpty(Place) ? "-" : Place;
            tbDescription.Text = string.IsNullOrEmpty(Description) ? "-" : Description;
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (OperationCount < Count)
            {
                OperationCount++;
                UpdateButtonsState();
            }
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (OperationCount > 0)
            {
                OperationCount--;
                UpdateButtonsState();
            }
        }

        private void UpdateButtonsState()
        {
            btnPlus.IsEnabled = OperationCount < Count;
            btnMinus.IsEnabled = OperationCount > 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OperationCount == 0)
                return;

            scanQRModel.MovementProduct(Id, OperationCount, user.Id, 1);

            Count -= OperationCount;

            UpdateUI();
        }
    }
}
