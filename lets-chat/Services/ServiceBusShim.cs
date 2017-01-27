using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.ServiceBus;
using System;

namespace lets_chat.Services
{
    public class ServiceBusShim : IServiceBusShim
    {
        private NamespaceManager _manager;
        private MessagingFactory _factory;

        public ServiceBusShim(NamespaceManager manager, MessagingFactory factory)
        {
            _manager = manager;
            _factory = factory;
        }

        public Task<SubscriptionDescription> CreateSubscriptionAsync(SubscriptionDescription description)
        {
            return _manager.CreateSubscriptionAsync(description);
        }

        public SubscriptionClient CreateSubscriptionClient(string topicPath, string name)
        {
            return _factory.CreateSubscriptionClient(topicPath, name);
        }

        public Task<TopicDescription> CreateTopicAsync(TopicDescription description)
        {
            return _manager.CreateTopicAsync(description);
        }

        public TopicClient CreateTopicClient(string path)
        {
            return _factory.CreateTopicClient(path);
        }

        public Task DeleteSubscriptionAsync(string topicPath, string name)
        {
            return _manager.DeleteSubscriptionAsync(topicPath, name);
        }

        public Task DeleteTopicAsync(string path)
        {
            return _manager.DeleteTopicAsync(path);
        }

        public Task<bool> TopicExistsAsync(string path)
        {
            return _manager.TopicExistsAsync(path);
        }

        public Task<bool> SubscriptionExistsAsync(string topicPath, string name)
        {
            return _manager.SubscriptionExistsAsync(topicPath, name);
        }

        public TopicDescription GetTopic(string topicPath)
        {
            return _manager.GetTopic(topicPath);
        }
    }
}
