using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QRcodeStorage.Views.UserControls
{
    public partial class ucFileInfo : UserControl
    {
        public static readonly DependencyProperty IsSuccessProperty =
         DependencyProperty.Register("IsSuccess", typeof(bool), typeof(ucFileInfo),
             new PropertyMetadata(true, OnPropertyChanged));

        public static readonly DependencyProperty QrInfoProperty =
            DependencyProperty.Register("QrInfo", typeof(string), typeof(ucFileInfo),
                new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(ucFileInfo),
                new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty InfoProperty =
            DependencyProperty.Register("Info", typeof(string), typeof(ucFileInfo),
                new PropertyMetadata(string.Empty, OnPropertyChanged));

        public static readonly DependencyProperty FileSizeProperty =
            DependencyProperty.Register("FileSize", typeof(long), typeof(ucFileInfo),
                new PropertyMetadata(0L, OnPropertyChanged));

        public bool IsSuccess
        {
            get { return (bool)GetValue(IsSuccessProperty); }
            set { SetValue(IsSuccessProperty, value); }
        }

        public string QrInfo
        {
            get { return (string)GetValue(QrInfoProperty); }
            set { SetValue(QrInfoProperty, value); }
        }

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public long FileSize
        {
            get { return (long)GetValue(FileSizeProperty); }
            set { SetValue(FileSizeProperty, value); }
        }

        public string Info
        {
            get { return (string)GetValue(InfoProperty); }
            set { SetValue(InfoProperty, value); }
        }

        public ucFileInfo()
        {
            InitializeComponent();
            DataContext = this;
            UpdateUI();
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ucFileInfo)d;
            control.UpdateUI();
        }
        public string FormatFileSize(long bytes)
        {
            string[] sizes = { "Б", "КБ", "МБ", "ГБ" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:F2} {sizes[order]}";
        }
        private void UpdateUI()
        {

            tbFileSize.Text = FormatFileSize(FileSize);
            tbPath.Text = FilePath;
            tbQrInfo.Text = QrInfo;
            tbOperationType.Text = Info;
            

            if (!IsSuccess)
            {
                brBack.BorderBrush = new SolidColorBrush(Color.FromRgb(244, 67, 54));
                icon.Fill = new SolidColorBrush(Color.FromRgb(244, 67, 54));
                brPath.BorderBrush = new SolidColorBrush(Color.FromRgb(244, 67, 54));
                brPath.Background = new SolidColorBrush(Color.FromRgb(53, 47, 56));
            }
            else
            {
                brBack.BorderBrush = new SolidColorBrush(Color.FromRgb(65, 204, 116));
                icon.Fill = new SolidColorBrush(Color.FromRgb(65, 204, 116));
                brPath.BorderBrush = new SolidColorBrush(Color.FromRgb(65, 204, 116));
                brPath.Background = new SolidColorBrush(Color.FromRgb(22, 40, 41));
            }
        }
    }
}
