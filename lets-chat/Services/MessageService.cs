using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;

namespace lets_chat
{
    public class MessageService : IMessageService
    {
        //TODO: Make async
        private string _connectionString;
        private const string Topic = "ChatRoom";
        private static NamespaceManager _manager;
        private static SubscriptionClient _subscriptionClient;
        private TopicClient _topicClient;
        public event EventHandler<string> MessageReceived;

        public void Start()
        {
            _connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            _manager = NamespaceManager.CreateFromConnectionString(_connectionString);
            CreateTopic();
            CreateTopicClient();
        }

        private static void CreateTopic()
        {
            if (!_manager.TopicExists(Topic))
            {
                _manager.CreateTopic(Topic);
            }
        }             

        public void Stop()
        {
            _topicClient.Close();

            if (_subscriptionClient != null)
            {
                _subscriptionClient.Close();
            }            
        }

        private void ReceiveMessage(BrokeredMessage msg)
        {
            var body = msg.GetBody<string>();
            MessageReceived?.Invoke(this, body);
            msg.Complete();
        }

        public void SendMessage(string msg)
        {
            var brokeredMsg = new BrokeredMessage(msg);
            _topicClient.Send(brokeredMsg);
        }

        public void RegisterUser(string user)
        {
            CreateUserSubscription(user);
            CreateSubscriptionClient(user);
        }

        private static void CreateUserSubscription(string subscription)
        {
            if (!_manager.SubscriptionExists(Topic, subscription))
            {
                _manager.CreateSubscription(Topic, subscription);
            }
        }

        private void CreateSubscriptionClient(string subscription)
        {
            _subscriptionClient = SubscriptionClient
                .CreateFromConnectionString(_connectionString, Topic, subscription);
            _subscriptionClient.OnMessage(msg => ReceiveMessage(msg));
        }

        private void CreateTopicClient()
        {
            _topicClient = TopicClient.CreateFromConnectionString(_connectionString, Topic);
        }
    }
}
