using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using SignalRChat.Interface;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        Microsoft.AspNetCore.SignalR.Client.HubConnection connection;

        public MainWindow()
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("https://192.168.1.19:44326/ChatHub")
                .AddMessagePackProtocol()
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await connection.StartAsync();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var data = await connection.InvokeAsync<byte[]>("GetData");
            stopwatch.Stop();
            using (var stream = new MemoryStream(data))
            {
                //var bitmap = new Bitmap(stream);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = stream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                Image_Local.Source = bitmapImage;
                TextBox_Local.Text = stopwatch.ElapsedMilliseconds.ToString();
            }
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
