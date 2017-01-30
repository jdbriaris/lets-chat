using System;
using System.Threading.Tasks;

namespace lets_chat
{
    public interface IMessageService
    {
        void Initialize();
        Task Stop();
        void SendMessage(string msg);
        void RegisterUser(string user);
        Task UnregisterUser();
        event EventHandler<string> MessageReceived;
        event EventHandler UserRegistered;
        event EventHandler UserUnregistered;
    }
}
