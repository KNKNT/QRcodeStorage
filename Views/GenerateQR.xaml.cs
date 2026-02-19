using Microsoft.Win32;
using QRcodeStorage.Models;
using QRcodeStorage.Services;
using QRcodeStorage.Views;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.Windows.Compatibility;


namespace QRcodeStorage.Pages
{
    public partial class GenerateQR : Page
    {
        Loader loader = new();
        Dictionary<string, int> productQuantities = new();
        DataView dataView;
        Product product = new();
        int size = 100, columns = 5, rows = 8, maxCount = 40;
        int totalCodesCount = 0;
        List<int> idProducts = new();


        private void cbShowQr_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e) => cbShowQr.IsChecked = !cbShowQr.IsChecked;
        private void cbShowQr_Checked(object sender, RoutedEventArgs e) => dataView.RowFilter = "[Qr] = '0'";
        private void cbShowQr_Unchecked(object sender, RoutedEventArgs e) => dataView.RowFilter = null;
        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateQr();

        public GenerateQR()
        {
            InitializeComponent();
            UpdateTable();
        }
        private void UpdateTable()
        {
            dataView = loader.LoadDataTable("SELECT id_product, name, maker, qr FROM ShowProducts");
            dgProducts.ItemsSource = dataView;
        }
        private void UpdateQr()
        {
            qrUniformGrid.Children.Clear();
            stCount.Children.Clear();
            totalCodesCount = 0;


            foreach (var selectedItem in dgProducts.SelectedItems)
            {
                if (selectedItem is DataRowView selectedRow)
                {
                    var productName = selectedRow[1].ToString();
                    var counter = new ucCountOfCodes(productName);

                    counter.CountChanged += (sender, count) =>
                    {
                        var control = sender as ucCountOfCodes;
                        if (control != null)
                        {
                            int oldCount = productQuantities.ContainsKey(control.ProductName)
                                ? productQuantities[control.ProductName]
                                : 0;

                            int delta = count - oldCount;

                            if (totalCodesCount + delta <= maxCount || delta < 0)
                            {
                                totalCodesCount += delta;
                                productQuantities[control.ProductName] = count;
                                RecreateQrCodes();
                                UpdateCountersState();
                            }
                            else
                            {
                                control.Count = oldCount;
                            }
                        }
                    };

                    stCount.Children.Add(counter);
                    productQuantities[productName] = counter.Count;
                    totalCodesCount += counter.Count;
                }
            }

            UpdateCountersState();
            RecreateQrCodes();
        }

        private void UpdateCountersState()
        {
            bool maxReached = totalCodesCount >= maxCount;

            foreach (var child in stCount.Children)
            {
                if (child is ucCountOfCodes counter)
                {
                    counter.SetMaxReached(maxReached);
                }
            }
            UpdateLimitInfo();
        }

        private void UpdateLimitInfo()
        {
            tbLimitInfo.Text = $" {totalCodesCount}/{maxCount}";
            tbLimitInfo.Foreground = totalCodesCount >= maxCount ? Brushes.Red : Brushes.Black;

            btnExport.IsEnabled = totalCodesCount > maxCount ? false : true;
            btnExport.IsEnabled = totalCodesCount == 0 ? false : true;

            if (totalCodesCount >= maxCount)
                tbLimitInfo.Text += " (Достигнут лимит!)";
        }

        private void RecreateQrCodes()
        {
            qrUniformGrid.Children.Clear();
            idProducts.Clear();

            foreach (var selectedItem in dgProducts.SelectedItems)
            {
                if (selectedItem is DataRowView selectedRow)
                {
                    product = new Product()
                    {
                        Id = Convert.ToInt32(selectedRow[0]),
                        Name = selectedRow[1].ToString(),
                    };

                    int quantity = 1;
                    if (productQuantities.ContainsKey(product.Name))
                    {
                        quantity = productQuantities[product.Name];
                    }

                    for (int i = 0; i < quantity; i++)
                    {
                        var stackPanel = new StackPanel()
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top
                        };

                        var border = new Border()
                        {
                            Margin = new Thickness(10, 0, 10, 0),
                        };

                        var qrImage = GenerateQRCode($"{product.Id}");

                        var image = new System.Windows.Controls.Image()
                        {
                            Source = qrImage,
                            Stretch = Stretch.UniformToFill,
                            Width = size,
                            Height = size
                        };

                        var textBlock = new TextBlock()
                        {
                            Text = quantity > 1 ? $"{product.Name} ({i + 1}/{quantity})" : product.Name,
                            TextAlignment = TextAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Top,
                            MaxWidth = size - 10,
                            TextTrimming = TextTrimming.CharacterEllipsis,
                            Margin = new Thickness(0, -10, 0, 0),
                            FontWeight = FontWeights.Medium
                        };

                        idProducts.Add(product.Id);
                        border.Child = image;
                        stackPanel.Children.Add(border);
                        stackPanel.Children.Add(textBlock);
                        qrUniformGrid.Children.Add(stackPanel);
                    }
                }
            }
        }

        private void cbSize_DropDownClosed(object sender, EventArgs e)
        {
            (size, columns, rows, maxCount) = cbSize.SelectedIndex switch
            {
                0 => (100, 5, 8, 40),
                1 => (180, 3, 4, 12),
                2 => (270, 2, 3, 6),
                _ => (100, 5, 8, 40)
            };
            qrUniformGrid.Columns = columns;
            qrUniformGrid.Rows = rows;
            totalCodesCount = 0;
            UpdateQr();
        }
        private WriteableBitmap GenerateQRCode(string text)
        {
            var barcodeWriter = new BarcodeWriter<WriteableBitmap>
            {
                Format = BarcodeFormat.QR_CODE,
                Renderer = new WriteableBitmapRenderer(),

                Options = new QrCodeEncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    CharacterSet = "UTF-8",
                    ErrorCorrection = ErrorCorrectionLevel.H
                },
            };

            return barcodeWriter.Write(text);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = "Qr";                 
            saveFileDialog.DefaultExt = ".png";                
            saveFileDialog.Filter = "PNG images (*.png)|*.png"; 
            saveFileDialog.Title = "Сохранить изображение в формате PNG";

            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = saveFileDialog.FileName;

                Border borderToExport = qrContainerBorder;
                borderToExport.BorderBrush = null;
                borderToExport.UpdateLayout();

                int width = (int)borderToExport.ActualWidth;
                int height = (int)borderToExport.ActualHeight;
                int dpi = 96;

                RenderTargetBitmap renderTarget = new RenderTargetBitmap(
                    width, height, dpi, dpi, PixelFormats.Pbgra32);
                renderTarget.Render(borderToExport);

                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        PngBitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(renderTarget));
                        encoder.Save(fs);
                    }

                    product.UpdateStatus(idProducts);
                    UpdateTable();

                    Notification.Show(true, "Успех", $"{filePath} сохранён");
                }
                catch (Exception ex)
                {
                    Notification.Show(false, "Ошибка", $"Ошибка при сохранении файла:\n{ex.Message}");
                }

                borderToExport.BorderBrush = Brushes.Gray;
                borderToExport.UpdateLayout();
            }
            else
            {
                Notification.Show(false, "Сохранение отменено", "");
            }
        }
    }
}
