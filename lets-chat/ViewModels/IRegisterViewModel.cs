using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace lets_chat.ViewModels
{
    public interface IRegisterViewModel
    {
        ICommand RegisterUserCommand { get; }
    }
}
