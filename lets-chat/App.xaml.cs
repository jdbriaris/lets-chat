using Microsoft.Practices.Unity;
using System.Windows;

namespace lets_chat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer _container;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _container = new UnityContainer();

            Bootstrapper.RegisterTypes(_container);

            var appView = _container.Resolve<AppView>();
            appView.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var messageService = _container.Resolve<IMessageService>();
            messageService.Stop();
        }
    }
}
