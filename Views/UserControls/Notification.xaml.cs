using Google.Protobuf.WellKnownTypes;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QRcodeStorage.Views
{
    /// <summary>
    /// Логика взаимодействия для Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public Notification()
        {
            InitializeComponent();            
        }
        public static void Show(bool isSuccess, string Header, string Caption)
        {
            Notification notification = new Notification();

            notification.tbHeader.Text = Header;
            notification.tbCaption.Text = Caption;
            notification.icon.Fill = isSuccess
            ? (SolidColorBrush)Application.Current.Resources["Accept"]
            : (SolidColorBrush)Application.Current.Resources["Exit"];

            notification.Left = SystemParameters.PrimaryScreenWidth - notification.Width;
            notification.Top = SystemParameters.PrimaryScreenHeight - notification.Height;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += (s, e) =>
            {
                timer.Stop();

                DoubleAnimation fadeOutAnimation = new DoubleAnimation
                {
                    From = 1.0,
                    To = 0.0,
                    Duration = TimeSpan.FromSeconds(1),
                    FillBehavior = FillBehavior.HoldEnd
                };

                fadeOutAnimation.Completed += (sender, e) => notification.Close();

                notification.BeginAnimation(Window.OpacityProperty, fadeOutAnimation);
            };
            timer.Start();

            notification.Show();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e) => DragMove();
        private void Button_Click(object sender, RoutedEventArgs e) => Close();

    }
}
