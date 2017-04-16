using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using SignalRChat.Lib;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        private readonly IConnectionMapping<string> _connections;

        public ChatHub(IConnectionMapping<string> connections)
        {
            if (connections == null)
            {
                throw new ArgumentNullException(nameof(connections));
            }
            _connections = connections;
        }

        public void SendBroadcastMessage(ChatMessage message)
        {
            Clients.All.BroadcastMessage(message);
        }

        public void SendDirectMessage(string toUsername, ChatMessage message)
        {
            foreach (var connectionId in _connections.GetConnections(toUsername))
            {
                Clients.Client(connectionId).DirectMessage(message);
            }
        }

        public override Task OnConnected()
        {
            string name = Context.QueryString["username"];

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.QueryString["username"];

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.QueryString["username"];

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}