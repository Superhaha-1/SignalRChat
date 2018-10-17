using Microsoft.AspNetCore.SignalR;
using SignalRChat.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            //await Clients.All.ReceiveMessage(user, message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        //public string GetData()
        //{
        //    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SignalRChat.Test.bmp"))
        //    {
        //        var length = stream.Length;
        //        using (BinaryReader br = new BinaryReader(stream))
        //        {
        //            return Convert.ToBase64String(br.ReadBytes((int)length));
        //        }
        //    }
        //}

        public async Task<byte[]> GetData()
        {
            using (var stream = new FileStream(@"D:\Share\111.bmp", FileMode.Open))
            {
                var length = stream.Length;
                using (BinaryReader br = new BinaryReader(stream))
                {
                    return await Task.Run(() => br.ReadBytes((int)length));
                }
            }
        }

        //public ChannelReader<byte> Counter()
        //{
        //    var channel = Channel.CreateUnbounded<byte>();

        //    // We don't want to await WriteItems, otherwise we'd end up waiting 
        //    // for all the items to be written before returning the channel back to
        //    // the client.
        //    var _ = WriteData(channel.Writer);
        //    return channel.Reader;
        //}

        //private async Task WriteData(ChannelWriter<byte> writer)
        //{
        //    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SignalRChat.Test.png"))
        //    {
        //        int b = 0;
        //        while ((b = stream.ReadByte()) != -1)
        //        {
        //            await writer.WriteAsync((byte)b);
        //        }
        //    }
        //    writer.TryComplete();
        //}
    }
}
