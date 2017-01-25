﻿using System;

namespace lets_chat
{
    public interface IMessageService
    {
        void Start();
        void Stop();
        void SendMessage(string msg);
        void RegisterUser(string user);
        event EventHandler<string> MessageReceived;
    }
}