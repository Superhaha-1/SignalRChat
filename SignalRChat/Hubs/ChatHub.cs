using Microsoft.AspNetCore.SignalR;
using SignalRChat.Interface;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public byte[] GetData()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SignalRChat.Test.png"))
            {
                int b = 0;
                var data = new List<byte>();
                while ((b = stream.ReadByte()) != -1)
                {
                    data.Add((byte)b);
                }
                return data.ToArray();
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
