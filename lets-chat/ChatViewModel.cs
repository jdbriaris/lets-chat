using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Input;

namespace lets_chat
{
    public class ChatViewModel : ViewModelBase
    {
        private IMessageService _messageService;
        private string _chat;
        public ICommand SendMessageCommand { get; set; }
        public string Message { get; set; }

        public ChatViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            SendMessageCommand = new RelayCommand<string>((msg) => SendMessage(msg));

            _messageService.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, string e)
        {
            Console.WriteLine(e);
            Chat = e;
        }

        public string Chat
        {
            get
            {
                return _chat;
            }

            set
            {
                _chat = value;
                RaisePropertyChanged();
            }
        }


        private void SendMessage(string msg)
        {
            _messageService.SendMessage(msg);
        }

    }
}
