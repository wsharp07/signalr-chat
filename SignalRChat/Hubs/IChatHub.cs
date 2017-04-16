using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalRChat.Lib;

namespace SignalRChat.Hubs
{
    public interface IChatHub
    {
        void SendBroadcastMessage(ChatMessage message);
        void SendDirectMessage(string toUsername, ChatMessage message);
    }
}
