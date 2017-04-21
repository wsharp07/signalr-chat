using System.Collections.Generic;

namespace SignalRChat.Server.Services
{
    public interface IConnectionMapping<T>
    {
        void Add(T key, string connectionId);
        IEnumerable<string> GetConnections(T key);
        void Remove(T key, string connectionId);
    }
}
