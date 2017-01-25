using System;

namespace lets_chat
{
    public interface IMessageService
    {
        void Start();
        void Stop();
        void SendMessage(string msg);
        event EventHandler<string> MessageReceived;
    }
}
