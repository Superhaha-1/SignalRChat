using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SignalRChat_Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("https://192.168.1.19:8001/ChatHub", options=>
                {
                })
                .AddMessagePackProtocol()
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            //忽略证书
            ServicePointManager.ServerCertificateValidationCallback
                      += (sender, cert, chain, error) => true;

            //连接
            connection.StartAsync();
        }

        private void SetImage(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                Image_Local.Source = bitmapImage;
            }
        }

        private async void Button_WebApi_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var data = await connection.InvokeAsync<byte[]>("GetData");
            stopwatch.Stop();
            TextBox_Local.Text = stopwatch.ElapsedMilliseconds.ToString();
            SetImage(data);
        }

        private void Button_Share_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            byte[] data = null;
            using (var stream = new FileStream(@"\\192.168.1.19\share\111.bmp", FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    data = br.ReadBytes((int)stream.Length);
                }
            }
            stopwatch.Stop();
            TextBox_Local.Text = stopwatch.ElapsedMilliseconds.ToString();
            SetImage(data);
        }

        //public async Task ReceiveMessage(string user, string message)
        //{
        //    await Task.Run(() =>
        //    {
        //        Dispatcher.Invoke(() =>
        //        {
        //            var newMessage = $"{user}: {message}";
        //            messagesList.Items.Add(newMessage);
        //        });
        //    }); 
        //}

        //private async void connectButton_Click(object sender, RoutedEventArgs e)
        //{
        //    connection.On<string, string>("ReceiveMessage", (user, message) =>
        //    {
        //        this.Dispatcher.Invoke(() =>
        //        {
        //            var newMessage = $"{user}: {message}";
        //            messagesList.Items.Add(newMessage);
        //        });
        //    });

        //    try
        //    {
        //        await connection.StartAsync();
        //        messagesList.Items.Add("Connection started");
        //        connectButton.IsEnabled = false;
        //        sendButton.IsEnabled = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        messagesList.Items.Add(ex.Message);
        //    }
        //}

        //private async void sendButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var data = await connection.InvokeAsync<byte[]>("GetData");
        //        using (var stream = new MemoryStream(data))
        //        {
        //            var bitmap = new Bitmap(stream);
        //        }

        //        await connection.InvokeAsync("SendMessage",
        //            userTextBox.Text, messageTextBox.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        messagesList.Items.Add(ex.Message);
        //    }
        //}
    }
}
