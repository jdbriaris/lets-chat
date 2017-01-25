using System.Windows.Input;

namespace lets_chat.ViewModels
{
    public interface ISendMessageViewModel
    {
        ICommand SendMessageCommand { get; }
    }
}
