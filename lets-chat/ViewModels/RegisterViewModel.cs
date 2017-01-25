using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace lets_chat.ViewModels
{
    public class RegisterViewModel : ViewModelBase, IRegisterViewModel
    {
        private IMessageService _messageService;

        public RegisterViewModel(IMessageService messageService)
        {
            _messageService = messageService;

            RegisterUserCommand = new RelayCommand<string>(user => RegisterUser(user));
        }

        public ICommand RegisterUserCommand { get; private set; }

        private void RegisterUser(string user)
        {
            _messageService.RegisterUser(user);
        }
    }
}
