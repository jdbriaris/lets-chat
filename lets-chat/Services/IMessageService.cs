using System;
using System.Threading.Tasks;

namespace lets_chat
{
    public interface IMessageService
    {
        void Initialize();
        void Stop();
        void SendMessage(string msg);
        void RegisterUser(string user);
        void UnregisterUser();
        event EventHandler<string> MessageReceived;
        event EventHandler UserRegistered;
        event EventHandler UserUnregistered;
    }
}
