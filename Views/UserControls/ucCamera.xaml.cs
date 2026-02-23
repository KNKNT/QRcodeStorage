using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.WpfExtensions;
using QRcodeStorage.Services;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ZXing.Windows.Compatibility;

namespace QRcodeStorage.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ucCamera.xaml
    /// </summary>
    public partial class ucCamera : UserControl
    {
        private Loader loader = new();
        private BarcodeReader barcodeReader;
        private VideoCapture capture;
        private DispatcherTimer timer;
        private bool isCameraReady = false;
        private bool isScanningEnabled = true;

        public ucCamera()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.Loaded += ucCamera_Loaded;
                this.Unloaded += ucCamera_Unloaded;
            }
        }
        private async void ucCamera_Unloaded(object sender, RoutedEventArgs e) => await StopCameraAsync(); 
        public async void ucCamera_Loaded(object sender, RoutedEventArgs e) => await InitializeCameraAsync();

        public async Task InitializeCameraAsync()
        {
            ShowLoading(true);
            await Task.Run(() =>
            {
                try
                {
                    capture = new VideoCapture(0);

                    if (!capture.IsOpened())
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            capture = new VideoCapture(i);
                            if (capture.IsOpened())
                                break;
                        }
                    }

                    isCameraReady = capture != null && capture.IsOpened();
                }
                catch (Exception ex)
                {
                    Notification.Show(false, "Ой", $"Ошибка инициализации камеры: {ex.Message}");
                    isCameraReady = false;
                }
            });

            Dispatcher.Invoke(() =>
            {
                if (isCameraReady)
                {
                    barcodeReader = new BarcodeReader();

                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(60);
                    timer.Tick += Timer_Tick;
                    timer.Start();

                    ShowLoading(false);
                    brCameraTip.Visibility = Visibility.Visible;

                }
                else
                {
                    LoadingText.Text = "Не удалось открыть камеру";
                    brCameraTip.Visibility = Visibility.Collapsed;
                    LoadingSpinner.Visibility = Visibility.Collapsed;
                }
            });
        }

        public Task StopCameraAsync()
        {
            imgCamera.Source = null;
            return Task.Run(() =>
            {
                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        timer?.Stop();
                        timer = null;
                    });

                    capture?.Release();
                    capture?.Dispose();
                    capture = null;
                }
                catch (Exception ex)
                {
                    Notification.Show(false, "Ой", $"Ошибка остановки камеры: {ex.Message}");
                }
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (capture != null && capture.IsOpened())
            {
                using (Mat frame = new Mat())
                {
                    capture.Read(frame);
                    if (!frame.Empty())
                    {
                        imgCamera.Source = frame.ToBitmapSource();
                        if (isScanningEnabled)
                            ScanQRCode(frame);
                    }
                }
            }
        }

        private void ScanQRCode(Mat frame)
        {
            using (var bitmap = frame.ToBitmap())
            {
                var result = barcodeReader.Decode(bitmap);
                if (result != null)
                {
                    (bool qrExist, DataView dataView) = loader.CheckAndLoadProduct(result.ToString());

                    if (qrExist)
                    {
                        Console.Beep(820, 300);
                        Notification.Show(true, "Найден QR-код", $"Обнаружен '{result.Text}'");
                        Coldown();
                    }
                    else
                    {
                        Console.Beep(250, 300);
                        Notification.Show(false, "QR не распознан", $"'{result.Text}' Не существует");
                        Coldown();
                    }
                }
            }
        }

        private void Coldown(int seconds = 1)
        {
            isScanningEnabled = false;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(seconds)
            };
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                isScanningEnabled = true;
            };
            timer.Start();
        }

        private void ShowLoading(bool show)
        {
            brLoadingOverlay.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UserControl_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            bool isEnabled = (bool)e.NewValue;

            if (isEnabled)
                InitializeCameraAsync();
            
            else
                StopCameraAsync();  
            
        }
    }
}
