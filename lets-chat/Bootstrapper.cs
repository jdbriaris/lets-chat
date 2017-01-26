using lets_chat.ViewModels;
using Microsoft.Practices.Unity;

namespace lets_chat
{
    public class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            var messageService = new MessageService();
            messageService.Start();

            container.RegisterInstance(typeof(IMessageService), messageService);

            container.RegisterType<ISendMessageViewModel, SendMessageViewModel>();
            container.RegisterType<IReceiveMessageViewModel, ReceiveMessageViewModel>();
            container.RegisterType<IRegisterViewModel, RegisterViewModel>();

            container.RegisterType<ChatViewModel>();
            container.RegisterType<AppViewModel>(new InjectionConstructor(typeof(ChatViewModel)));
            container.RegisterType<AppView>(new InjectionConstructor(typeof(AppViewModel)));
        }
    }
}
