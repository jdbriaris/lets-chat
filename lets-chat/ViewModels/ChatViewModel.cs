using GalaSoft.MvvmLight;
using lets_chat.ViewModels;

namespace lets_chat
{
    public class ChatViewModel : ViewModelBase
    {
        public IRegisterViewModel RegisterViewModel { get; private set; }
        public ISendMessageViewModel SendMessageViewModel { get; private set; }
        public IReceiveMessageViewModel ReceiveMessageViewModel { get; private set; }

        public ChatViewModel(IRegisterViewModel registerViewModel, ISendMessageViewModel sendMessageViewModel, 
            IReceiveMessageViewModel receiveMessageViewModel)
        {
            RegisterViewModel = registerViewModel;
            SendMessageViewModel = sendMessageViewModel;
            ReceiveMessageViewModel = receiveMessageViewModel;
        }

    }
}
