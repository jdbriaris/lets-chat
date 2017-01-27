using lets_chat.Services;
using lets_chat.ViewModels;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;

namespace lets_chat
{
    public class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            var connString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            var topic = "ChatRoom";

            var manager = NamespaceManager.CreateFromConnectionString(connString);
            var factory = MessagingFactory.CreateFromConnectionString(connString);

            container.RegisterInstance(typeof(IServiceBusShim), new ServiceBusShim(manager, factory));
            container.RegisterType<IMessageService, MessageService>(new ContainerControlledLifetimeManager(), 
                new InjectionConstructor(typeof(IServiceBusShim), topic));

            container.RegisterType<ISendMessageViewModel, SendMessageViewModel>();
            container.RegisterType<IReceiveMessageViewModel, ReceiveMessageViewModel>();
            container.RegisterType<IRegisterViewModel, RegisterViewModel>();

            container.RegisterType<ChatViewModel>();
            container.RegisterType<AppViewModel>(new InjectionConstructor(typeof(ChatViewModel)));
            container.RegisterType<AppView>(new InjectionConstructor(typeof(AppViewModel)));
        }
    }
}
