using System;
using System.Threading.Tasks;

namespace SignalRChat.Interface
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
}
