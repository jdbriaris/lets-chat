using System.Windows.Input;

namespace lets_chat.ViewModels
{
    public interface IRegisterViewModel
    {
        string ButtonText { get; }
        string UserName { get; set; }
        bool IsEnterButtonEnabled { get; }
        bool IsEnterNameTextBoxEnabled { get; }
        ICommand EnterLeaveChatRoomCommand { get; }
    }
}
