using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;

namespace lets_chat.ViewModels
{
    public class RegisterViewModel : ViewModelBase, IRegisterViewModel
    {
        private string _buttonText;
        private string _userName;
        private IMessageService _messageService;
        private bool _isEnterNameTextBoxEnabled;
        private ICommand _enterLeaveChatRoomCommand;

        public RegisterViewModel(IMessageService messageService)
        {
            _messageService = messageService;
            _messageService.UserRegistered += OnUserRegistered;
            _messageService.UserUnregistered += OnUserUnregistered;

            ButtonText = Properties.Resources.EnterChatRoom;
            IsEnterNameTextBoxEnabled = true;
            EnterLeaveChatRoomCommand = new RelayCommand(EnterChatRoom);
        }

        public ICommand EnterLeaveChatRoomCommand
        {
            get
            {
                return _enterLeaveChatRoomCommand;
            }
            private set
            {
                _enterLeaveChatRoomCommand = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsEnterButtonEnabled");
            }
        }

        public bool IsEnterButtonEnabled
        {
            get
            {
                return !string.IsNullOrEmpty(_userName);
            }
        }

        public string ButtonText
        {
            get
            {
                return _buttonText;
            }
            private set
            {
                _buttonText = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEnterNameTextBoxEnabled
        {
            get
            {
                return _isEnterNameTextBoxEnabled;
            }
            set
            {
                _isEnterNameTextBoxEnabled = value;
                RaisePropertyChanged();
            }
        }

        private void EnterChatRoom()
        {
            _messageService.RegisterUser(UserName);
        }

        private void LeaveChatRoom()
        {
            _messageService.UnregisterUser();
        }

        private void OnUserRegistered(object sender, EventArgs e)
        {
            ButtonText = Properties.Resources.LeaveChatRoom;
            IsEnterNameTextBoxEnabled = false;
            EnterLeaveChatRoomCommand = new RelayCommand(LeaveChatRoom);
        }

        private void OnUserUnregistered(object sender, EventArgs e)
        {
            ButtonText = Properties.Resources.EnterChatRoom;
            IsEnterNameTextBoxEnabled = true;
            EnterLeaveChatRoomCommand = new RelayCommand(EnterChatRoom);
            UserName = string.Empty;
        }
    }
}
