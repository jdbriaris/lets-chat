using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;

namespace lets_chat.Services
{
    public interface IServiceBusShim
    {
        Task<SubscriptionDescription> CreateSubscriptionAsync(SubscriptionDescription description);
        Task<TopicDescription> CreateTopicAsync(TopicDescription description);
        Task DeleteSubscriptionAsync(string topicPath, string name);
        Task DeleteTopicAsync(string path);
        void CreateTopicClient(string path);
        void CreateSubscriptionClient(string topicPath, string name);
        Task<bool> TopicExistsAsync(string path);
        Task<bool> SubscriptionExistsAsync(string topicPath, string name);
        Task SendMessageAsync(BrokeredMessage msg);
        Task CloseTopicClientAsync();
        Task CloseSubscriptionClientAsync();
        event EventHandler<BrokeredMessage> MessageReceived;
        int GetNumberOfActiveSubscriptions(string topicPath);
    }


}
