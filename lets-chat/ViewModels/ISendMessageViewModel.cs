using System.Windows.Input;

namespace lets_chat.ViewModels
{
    public interface ISendMessageViewModel
    {
        bool IsSendMessageEnabled { get; }
        ICommand SendMessageCommand { get; }
    }
}
