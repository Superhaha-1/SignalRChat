using System.Threading.Tasks;

namespace SignalRChat.Interface
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }

    public interface IChatServer
    {
        Task SendMessage(string user, string message);

        byte[] GetData();
    }
}
