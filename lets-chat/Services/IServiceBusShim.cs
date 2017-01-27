using Microsoft.ServiceBus.Messaging;
using System.Threading.Tasks;

namespace lets_chat.Services
{
    public interface IServiceBusShim
    {
        Task<SubscriptionDescription> CreateSubscriptionAsync(SubscriptionDescription description);
        Task<TopicDescription> CreateTopicAsync(TopicDescription description);
        Task DeleteSubscriptionAsync(string topicPath, string name);
        Task DeleteTopicAsync(string path);
        TopicClient CreateTopicClient(string path);
        SubscriptionClient CreateSubscriptionClient(string topicPath, string name);
        Task<bool> TopicExistsAsync(string path);
        Task<bool> SubscriptionExistsAsync(string topicPath, string name);
        TopicDescription GetTopic(string topicPath);
    }


}
