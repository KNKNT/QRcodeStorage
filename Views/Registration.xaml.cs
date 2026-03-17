using QRcodeStorage.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace QRcodeStorage.Views
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        bool showPassword = true;
        User user = new();
        MainWindow _mainWindow;

        public Registration(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        private void btShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (showPassword)
            {
                psPassword.Visibility = Visibility.Collapsed;
                tbPassword.Visibility = Visibility.Visible;

                tbPassword.Text = psPassword.Password;

                btShowPassword.Content = "🔒";
                showPassword = false;
            }
            else
            {
                psPassword.Visibility = Visibility.Visible;
                tbPassword.Visibility = Visibility.Collapsed;

                psPassword.Password = tbPassword.Text;

                btShowPassword.Content = "🔓";
                showPassword = true;
            }
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            if(user.LoginUser(tbLogin.Text, showPassword ? psPassword.Password : tbPassword.Text))
            {
                _mainWindow.NavigationFrame.Content = null;
                _mainWindow.rbCreateProduct.IsChecked = true;
                _mainWindow.gridSplitter.IsEnabled = true;
                _mainWindow.NavigationFrame.Opacity = 0;

                AnimateNavigationPanel();
            }
        }
        private void AnimateNavigationPanel()
        {
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = 260,
                Duration = TimeSpan.FromMilliseconds(700),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut } 
            };

            _mainWindow.cNavigationPanel.BeginAnimation(GridLengthAnimation.WidthProperty, widthAnimation);

            widthAnimation.Completed += (s, e) =>
            {
                _mainWindow.cNavigationPanel.MinWidth = 58;
                _mainWindow.cNavigationPanel.Width = new GridLength(260);
            };

            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,  
                Duration = TimeSpan.FromMilliseconds(1500),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            opacityAnimation.Completed += (s, e) =>
            {
                _mainWindow.NavigationFrame.Opacity = 1;  
            };

            _mainWindow.NavigationFrame.BeginAnimation(OpacityProperty, opacityAnimation);
        }
    }
    public static class GridLengthAnimation
    {
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.RegisterAttached(
                "Width",
                typeof(double),
                typeof(GridLengthAnimation),
                new PropertyMetadata(OnWidthChanged));

        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ColumnDefinition column)
            {
                column.Width = new GridLength((double)e.NewValue);
            }
            else if (d is Grid grid)
            {
                grid.Width = (double)e.NewValue;
            }
        }

        public static void SetWidth(DependencyObject element, double value)
        {
            element.SetValue(WidthProperty, value);
        }

        public static double GetWidth(DependencyObject element)
        {
            return (double)element.GetValue(WidthProperty);
        }
    }
}
