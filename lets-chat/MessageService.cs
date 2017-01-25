using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;

namespace lets_chat
{
    public class MessageService : IMessageService
    {
        //TODO: Make async

        //TODO: Connection string ought to go in some kind of configuration (use CloudConfigurationManager)
        private const string ServiceBusConnectionString = "Endpoint=sb://letschat.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZGT7w8X//XDEvww/R/XWt7RBLWnmBsxG+KpXdh2UyhU=";
        private const string Topic = "ChatRoom";
        private const string Subscription = "AllMessages";
        private TopicClient _topicClient;
        private SubscriptionClient _subscriptionClient;

        public event EventHandler<string> MessageReceived;

        public void Start()
        {
            var manager = NamespaceManager.CreateFromConnectionString(ServiceBusConnectionString);
            CreateTopic(manager);
            CreateSubsciption(manager);

            var factory = MessagingFactory.CreateFromConnectionString(ServiceBusConnectionString);
            _topicClient = factory.CreateTopicClient(Topic);
            _subscriptionClient = factory.CreateSubscriptionClient(Topic, Subscription);

            _subscriptionClient.OnMessage(msg => ReceiveMessage(msg));
        }

        private static void CreateSubsciption(NamespaceManager manager)
        {
            if (!manager.SubscriptionExists(Topic, Subscription))
            {
                manager.CreateSubscription(Topic, Subscription);
            }
        }

        private static void CreateTopic(NamespaceManager manager)
        {
            if (!manager.TopicExists(Topic))
            {
                manager.CreateTopic(Topic);
            }
        }

        public void Stop()
        {
            _topicClient.Close();
            _subscriptionClient.Close();
        }

        private void ReceiveMessage(BrokeredMessage msg)
        {
            var body = msg.GetBody<string>();
            MessageReceived?.Invoke(this, body);
        }

        public void SendMessage(string msg)
        {
            var brokeredMsg = new BrokeredMessage(msg);
            _topicClient.Send(brokeredMsg);
        }
    }
}
