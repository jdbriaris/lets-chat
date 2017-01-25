using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace lets_chat.ViewModels
{
    public class SendMessageViewModel : ViewModelBase, ISendMessageViewModel
    {
        private IMessageService _messageService;

        public SendMessageViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            SendMessageCommand = new RelayCommand<string>(msg => SendMessage(msg));
        }        

        public ICommand SendMessageCommand { get; private set; }

        private void SendMessage(string msg)
        {
            _messageService.SendMessage(msg);
        }
    }
}
