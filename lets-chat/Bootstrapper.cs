using lets_chat.ViewModels;
using Microsoft.Practices.Unity;
using System.Configuration;

namespace lets_chat
{
    public class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            var connString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var topic = "ChatRoom";
            container.RegisterInstance(typeof(IMessageService), new MessageService(connString, topic));

            container.RegisterType<ISendMessageViewModel, SendMessageViewModel>();
            container.RegisterType<IReceiveMessageViewModel, ReceiveMessageViewModel>();
            container.RegisterType<IRegisterViewModel, RegisterViewModel>();

            container.RegisterType<ChatViewModel>();
            container.RegisterType<AppViewModel>(new InjectionConstructor(typeof(ChatViewModel)));
            container.RegisterType<AppView>(new InjectionConstructor(typeof(AppViewModel)));
        }
    }
}
