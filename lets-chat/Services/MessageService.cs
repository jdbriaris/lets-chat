using lets_chat.Services;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;

namespace lets_chat
{
    public class MessageService : IMessageService
    {
        private readonly string _topic;
        private string _uniqueUserId;
        private string _user;
        public event EventHandler<string> MessageReceived;
        public event EventHandler UserRegistered;
        public event EventHandler UserUnregistered;
        private readonly IServiceBusShim _serviceBusShim;

        public MessageService(IServiceBusShim serviceBusShim, string topic)
        {
            _topic = topic;
            _serviceBusShim = serviceBusShim;                
        }

        public void Initialize()
        {
            CreateTopic();
            CreateTopicClient();
        }

        private async void CreateTopic()
        {
            var topicExists = await _serviceBusShim.TopicExistsAsync(_topic);
            if (!topicExists)
            {
                var description = new TopicDescription(_topic);
                await _serviceBusShim.CreateTopicAsync(description);
            }
        }

        private void CreateTopicClient()
        {
            _serviceBusShim.CreateTopicClient(_topic);
        }

        public async void RegisterUser(string user)
        {
            _user = user;
            _uniqueUserId = Guid.NewGuid().ToString();

            var subscriptionExists = await _serviceBusShim.SubscriptionExistsAsync(_topic, _uniqueUserId);
            if (!subscriptionExists)
            {
                var description = new SubscriptionDescription(_topic, _uniqueUserId);
                await _serviceBusShim.CreateSubscriptionAsync(description);
            }

            _serviceBusShim.CreateSubscriptionClient(_topic, _uniqueUserId);
            _serviceBusShim.MessageReceived += OnMessageReceived;

            UserRegistered?.Invoke(this, EventArgs.Empty);
        }

        private void OnMessageReceived(object sender, BrokeredMessage msg)
        {
            var body = msg.GetBody<string>();
            MessageReceived?.Invoke(this, body);
        }

        public async Task UnregisterUser()
        {
            await _serviceBusShim.CloseSubscriptionClientAsync();
            await _serviceBusShim.DeleteSubscriptionAsync(_topic, _uniqueUserId);
            UserUnregistered?.Invoke(this, EventArgs.Empty);
        }

        public async Task Stop()
        {
            await _serviceBusShim.CloseTopicClientAsync();
            if (_serviceBusShim.GetNumberOfActiveSubscriptions(_topic) == 0)
            {
                await _serviceBusShim.DeleteTopicAsync(_topic);
            }           
        }

        private async Task SendTopicClientMessage(string msg)
        {
            var brokeredMsg = new BrokeredMessage(msg);
            await _serviceBusShim.SendMessageAsync(brokeredMsg);
        }

        public async void SendMessage(string msg)
        {
            var namedMessage = _user + ">> " + msg;
            await SendTopicClientMessage(namedMessage);            
        }
               
    }
}
