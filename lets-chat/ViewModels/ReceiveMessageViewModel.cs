using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace lets_chat.ViewModels
{
    public class ReceiveMessageViewModel : ViewModelBase, IReceiveMessageViewModel
    {
        private string _message;
        private List<string> _messages = new List<string>();

        public ReceiveMessageViewModel(IMessageService messageService)
        {
            Messages = new ObservableCollection<string>();

            messageService.MessageReceived += OnMessageReceived;
        }

        public ObservableCollection<string> Messages { get; set; }        

        private void OnMessageReceived(object sender, string e)
        {
            Messages.Add(e);
        }
    }
}
