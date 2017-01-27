using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;

namespace lets_chat.ViewModels
{
    public class SendMessageViewModel : ViewModelBase, ISendMessageViewModel
    {
        private bool _isSendMessageEnabled;
        private IMessageService _messageService;

        public SendMessageViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            _messageService.UserRegistered += OnUserRegistered;

            SendMessageCommand = new RelayCommand<string>(msg => SendMessage(msg));
        }        

        public ICommand SendMessageCommand { get; private set; }

        public bool IsSendMessageEnabled
        {
            get
            {
                return _isSendMessageEnabled;
            }
            private set
            {
                _isSendMessageEnabled = value;
                RaisePropertyChanged();
            }
        }

        private void SendMessage(string msg)
        {
            _messageService.SendMessage(msg);
        }

        private void OnUserRegistered(object sender, EventArgs e)
        {
            IsSendMessageEnabled = true;
        }
    }
}
