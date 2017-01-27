using System.Collections.ObjectModel;

namespace lets_chat.ViewModels
{
    public interface IReceiveMessageViewModel
    {
        ObservableCollection<string> Messages { get; set; }
    }
}
