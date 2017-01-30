using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace lets_chat.ViewModels
{
    public class ReceiveMessageViewModel : ViewModelBase, IReceiveMessageViewModel
    {
        private List<string> _messages = new List<string>();

        public ReceiveMessageViewModel(IMessageService messageService)
        {
            Messages = new ObservableCollection<string>();

            messageService.MessageReceived += OnMessageReceived;
        }

        public ObservableCollection<string> Messages { get; set; }        

        private void OnMessageReceived(object sender, string e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                Messages.Add(e);
            });            
        }
    }
}
