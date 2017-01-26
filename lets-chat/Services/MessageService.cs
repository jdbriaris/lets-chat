using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace lets_chat
{
    public class MessageService : IMessageService
    {
        private readonly string _connectionString;
        private readonly string _topic;
        private static NamespaceManager _manager;
        private static MessagingFactory _factory;
        private static SubscriptionClient _subscriptionClient;
        private TopicClient _topicClient;
        public event EventHandler<string> MessageReceived;

        public MessageService(string serviceBusConnectionString, string topicPath)
        {
            _connectionString = serviceBusConnectionString;
            _topic = topicPath;
        }

        public async void Initialize()
        {
            _manager = NamespaceManager.CreateFromConnectionString(_connectionString);
            _factory = MessagingFactory.CreateFromConnectionString(_connectionString);

            await CreateTopic(_topic);
            _topicClient = _factory.CreateTopicClient(_topic);
        }

        public async void RegisterUser(string user)
        {
            var subscriptionExists = await _manager.SubscriptionExistsAsync(_topic, user);
            if (!subscriptionExists)
            {
                await _manager.CreateSubscriptionAsync(_topic, user);
            }
            _subscriptionClient = _factory.CreateSubscriptionClient(_topic, user);
            _subscriptionClient.OnMessageAsync((msg) => ReceiveMessage(msg));
        }

        public async void Stop()
        {
            await _topicClient.CloseAsync();
            if (_subscriptionClient != null)
            {
                await _subscriptionClient.CloseAsync();
            }
        }

        public async void SendMessage(string msg)
        {
            var brokeredMsg = new BrokeredMessage(msg);
            await _topicClient.SendAsync(brokeredMsg);
        }

        private async Task CreateTopic(string topicPath)
        {
            var topicExists = await _manager.TopicExistsAsync(topicPath);
            if (!topicExists)
            {
                await _manager.CreateTopicAsync(topicPath);
            }
        }

        private async Task ReceiveMessage(BrokeredMessage msg)
        {
            await Task.Run(() =>
            {
                var body = msg.GetBody<string>();
                Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    MessageReceived?.Invoke(this, body);
                }));
                msg.Complete();
            });            
        }     

    }
}
