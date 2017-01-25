using GalaSoft.MvvmLight;

namespace lets_chat.ViewModels
{
    public class ReceiveMessageViewModel : ViewModelBase, IReceiveMessageViewModel
    {
        private string _message;

        public ReceiveMessageViewModel(IMessageService messageService)
        {
            messageService.MessageReceived += OnMessageReceived;
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }

        private void OnMessageReceived(object sender, string e)
        {
            Message = e;
        }


    }
}
