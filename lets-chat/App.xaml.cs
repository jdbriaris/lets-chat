using System.Windows;

namespace lets_chat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IMessageService _messageService;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _messageService = new MessageService();
            _messageService.Start();

            var chatViewModel = new ChatViewModel(_messageService);

            var appViewModel = new AppViewModel();
            appViewModel.ViewModel = chatViewModel;

            var appView = new AppView(appViewModel);
            appView.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _messageService.Stop();
        }
    }
}
