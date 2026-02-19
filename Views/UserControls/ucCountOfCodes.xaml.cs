using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QRcodeStorage.Views
{
    public partial class ucCountOfCodes : UserControl
    {
        int count = 1; 
        private bool isMaxReached = false;
        public int Count
        {
            get => count;
            set
            {
                count = value;
                tblCount.Text = count.ToString();
            }
        }
        public string ProductName { get; private set; }
        public event EventHandler<bool> MaxReachedChanged;
        public event EventHandler<int> CountChanged;

        public ucCountOfCodes(string name)
        {
            InitializeComponent();
            ProductName = name;
            tblCount.Text = count.ToString();
            tblName.Text = name;
        }

        private void btnPlus_Click(object sender, RoutedEventArgs e)
        {
            Count++;
            CountChanged?.Invoke(this, Count);
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Count > 1)
            {
                Count--;
                CountChanged?.Invoke(this, Count);
            }
        }
        public void SetMaxReached(bool reached)
        {
            isMaxReached = reached;
            btnPlus.IsEnabled = !reached;

            btnPlus.Background = reached ? Brushes.Gray : Brushes.LightGray;
            btnPlus.Foreground = reached ? Brushes.Gray : Brushes.Black;

            MaxReachedChanged?.Invoke(this, reached);
        }
    }
}
