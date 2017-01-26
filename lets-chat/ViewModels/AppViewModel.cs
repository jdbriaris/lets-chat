using GalaSoft.MvvmLight;

namespace lets_chat
{
    public class AppViewModel : ViewModelBase
    {
        public AppViewModel(ViewModelBase startUpViewModel)
        {
            ViewModel = startUpViewModel;
        }

        public ViewModelBase ViewModel { get; set; }

    }
}
