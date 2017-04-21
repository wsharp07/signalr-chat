using SignalRChat.ServiceModel;

namespace SignalRChat.Server.Hubs
{
    public interface IChatHub
    {
        void SendBroadcastMessage(ChatMessage message);
        void SendDirectMessage(string toUsername, ChatMessage message);
    }
}
