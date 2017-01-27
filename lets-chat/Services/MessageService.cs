using lets_chat.Services;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace lets_chat
{
    public class MessageService : IMessageService
    {
        private readonly string _topic;
        private string _uniqueUserId;
        private string _user;
        private static SubscriptionClient _subscriptionClient;
        private TopicClient _topicClient;
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

            _subscriptionClient = _serviceBusShim.CreateSubscriptionClient(_topic, _uniqueUserId);
            _subscriptionClient.OnMessageAsync((msg) => ReceiveMessage(msg));

            var introMessage = ">>** " + user + " has entered the room **";
            await SendTopicClientMessage(introMessage);

            UserRegistered?.Invoke(this, EventArgs.Empty);
        }
        
        public async void UnregisterUser()
        {           
            var exitMessage = ">>** " + _user + " has left the room **";
            await SendTopicClientMessage(exitMessage);

            if (_subscriptionClient != null)
            {
                await _subscriptionClient.CloseAsync();
                await _serviceBusShim.DeleteSubscriptionAsync(_topic, _uniqueUserId);
            }      

            UserUnregistered?.Invoke(this, EventArgs.Empty);
        }

        public async void Stop()
        {
            await _topicClient.CloseAsync();
            var topicDescription = _serviceBusShim.GetTopic(_topic);
            var numberOfSubscriptions = topicDescription.SubscriptionCount;
            if (numberOfSubscriptions == 0)
            {
                await _serviceBusShim.DeleteTopicAsync(_topic);
            }           
        }

        private async Task SendTopicClientMessage(string msg)
        {
            var brokeredMsg = new BrokeredMessage(msg);
            await _topicClient.SendAsync(brokeredMsg);
        }

        public async void SendMessage(string msg)
        {
            var namedMessage = _user + ">> " + msg;
            await SendTopicClientMessage(namedMessage);            
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
            _topicClient = _serviceBusShim.CreateTopicClient(_topic);
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
