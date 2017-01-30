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
        private TopicClient _topicClient;
        private SubscriptionClient _subscriptionClient;
        public event EventHandler<BrokeredMessage> MessageReceived;

        public ServiceBusShim(NamespaceManager manager, MessagingFactory factory)
        {
            _manager = manager;
            _factory = factory;
        }

        public Task<SubscriptionDescription> CreateSubscriptionAsync(SubscriptionDescription description)
        {
            return _manager.CreateSubscriptionAsync(description);
        }

        public void CreateSubscriptionClient(string topicPath, string name)
        {
            _subscriptionClient = _factory.CreateSubscriptionClient(topicPath, name);
            _subscriptionClient.OnMessage((msg) =>
            {
                MessageReceived?.Invoke(this, msg);
            });
        }

        public Task<TopicDescription> CreateTopicAsync(TopicDescription description)
        {
            return _manager.CreateTopicAsync(description);
        }

        public void CreateTopicClient(string path)
        {
            _topicClient = _factory.CreateTopicClient(path);
        }

        public Task DeleteSubscriptionAsync(string topicPath, string name)
        {
            return _subscriptionClient != null 
                ? _manager.DeleteSubscriptionAsync(topicPath, name) : Task.CompletedTask;
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

        public Task SendMessageAsync(BrokeredMessage msg)
        {
            return _topicClient.SendAsync(msg);
        }

        public Task CloseTopicClientAsync()
        {
            return _topicClient.CloseAsync();
        }

        public Task CloseSubscriptionClientAsync()
        {
            return _subscriptionClient != null ?
                _subscriptionClient.CloseAsync() : Task.CompletedTask;           
        }

        public int GetNumberOfActiveSubscriptions(string topicPath)
        {
            var description = _manager.GetTopic(topicPath);
            return description.SubscriptionCount;
        }
    }
}
