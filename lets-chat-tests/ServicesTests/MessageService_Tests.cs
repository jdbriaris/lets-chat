using lets_chat;
using lets_chat.Services;
using Microsoft.ServiceBus.Messaging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lets_chat_tests.ServicesTests
{
    [TestFixture]
    public class MessageService_Tests
    {
        private const string Topic = "Topic";
        private IServiceBusShim _serviceBusShim;
        private IMessageService _messageService;

        [SetUp]
        public void TestInit()
        {
            _serviceBusShim = Substitute.For<IServiceBusShim>();
            _messageService = new MessageService(_serviceBusShim, Topic);
        }

        [Test]
        public void MessageService_Initialize_TopicDoesNotExist_CallsCreateTopicOnShimThenCallsCreateTopicClientOnShim()
        {
            _serviceBusShim.TopicExistsAsync(Topic).Returns(false);

            _messageService.Initialize();

            Received.InOrder(() =>
            {
                _serviceBusShim.Received(1).CreateTopicAsync(Arg.Is<TopicDescription>(td => td.Path.Equals(Topic)));
                _serviceBusShim.Received(1).CreateTopicClient(Topic);
            });            
        }

        [Test]
        public void MessageService_Initialize_TopicDoesExist_DoesNotCallCreateTopicOnShimButDoesCallCreateTopicClientOnShim()
        {
            _serviceBusShim.TopicExistsAsync(Topic).Returns(true);

            _messageService.Initialize();

            _serviceBusShim.DidNotReceive().CreateTopicAsync(Arg.Any<TopicDescription>());
            _serviceBusShim.Received(1).CreateTopicClient(Topic);
        }

        [Test]
        public void MessageService_RegisterUser_SubscriptionDoesNotExist_CallsCreateSubscriptionOnShimThenCallsCreateSubscriptionClientOnShim()
        {
            var subName = "A";
            var subClientName = "B";
            _serviceBusShim.SubscriptionExistsAsync(Topic, Arg.Any<string>()).Returns(false);
            _serviceBusShim.CreateSubscriptionAsync(Arg.Do<SubscriptionDescription>(sd => subName = sd.Name));
            _serviceBusShim.CreateSubscriptionClient(Arg.Any<string>(), Arg.Do<string>(name => subClientName = name));

            _messageService.RegisterUser("Bob");            

            Received.InOrder(() =>
            {
                _serviceBusShim.Received(1).CreateSubscriptionAsync(Arg.Is<SubscriptionDescription>(sd => sd.TopicPath.Equals(Topic)));
                _serviceBusShim.Received(1).CreateSubscriptionClient(Topic, Arg.Any<string>());
            });

            Assert.That(subName, Is.EqualTo(subClientName));
        }

        [Test]
        public void MessageService_RegisterUserWithSameName_SubscriptionDoesNotExist_CallsCreateSubscriptionOnShimWithUniqueName()
        {
            var names = new List<string>();
            _serviceBusShim.SubscriptionExistsAsync(Topic, Arg.Any<string>()).Returns(false);
            _serviceBusShim.CreateSubscriptionAsync(Arg.Do<SubscriptionDescription>(sd => names.Add(sd.Name)));

            _messageService.RegisterUser("Bob");
            _messageService.RegisterUser("Bob");

            Assert.That(names.Count, Is.EqualTo(2));
            Assert.That(names[0], Is.Not.EqualTo(names[1]));
        }

        [Test]
        public void MessageService_RegisterUser_SubscriptionDoesExist_DoesNotCallCreateSubscriptionOnShim_DoesCallCreateSubscriptionClientOnShim()
        {
            _serviceBusShim.SubscriptionExistsAsync(Topic, Arg.Any<string>()).Returns(true);

            _messageService.RegisterUser("Jim");

            _serviceBusShim.DidNotReceive().CreateSubscriptionAsync(Arg.Any<SubscriptionDescription>());
            _serviceBusShim.Received(1).CreateSubscriptionClient(Topic, Arg.Any<string>());
        }

        [Test]
        public void MessageService_RegisterUser_FiresUserRegisteredEvent()
        {
            var isFired = false;
            _messageService.UserRegistered += (sender, args) => { isFired = true; };

            _messageService.RegisterUser("Gary");

            Assert.IsTrue(isFired);
        }

        [Test]
        public void MessageService_RegisterUser_CallsCreateSubscriptionClientOnShim()
        {
            _messageService.RegisterUser("Gary");

            _serviceBusShim.Received(1).CreateSubscriptionClient(Topic, Arg.Any<string>());
        }

        [Test]
        public void MessageService_ShimRaisesMessageReceived_RaisesMessageReceived()
        {
            var expectedMsg = "This is a test message";
            var brokeredMsg = new BrokeredMessage(expectedMsg);
            var receivedMsg = "";
            var isFired = false;
            _messageService.MessageReceived += (sender, args) =>  {
                isFired = true;
                receivedMsg = args;
            };
            _messageService.RegisterUser("Barry");

            _serviceBusShim.MessageReceived += Raise.Event<EventHandler<BrokeredMessage>>(this, brokeredMsg);            

            Assert.That(isFired, Is.True);
            Assert.That(receivedMsg, Is.EqualTo(expectedMsg));
        }

        [Test]
        public async Task MessageService_UnregisterUser_CallsCloseSubscriptionClientAsyncOnShim()
        {
            await _messageService.UnregisterUser();

            await _serviceBusShim.Received(1).CloseSubscriptionClientAsync();
        }

        [Test]
        public async Task MessageService_UnregisterUser_CallsDeleteSubscriptionClientAsyncOnShim()
        {
            await _messageService.UnregisterUser();

            await _serviceBusShim.Received(1).DeleteSubscriptionAsync(Topic, Arg.Any<string>());
        }

        [Test]
        public async Task MessageService_UnregisterUser_RaisesUserUnregistered()
        {
            var isFired = false;
            _messageService.UserUnregistered += (sender, args) => { isFired = true; };

            await _messageService.UnregisterUser();

            Assert.That(isFired, Is.True);
        }

        [Test]
        public async Task MessageService_StopWithExistingSubscriptions_DoesNotCallCloseTopicClientAsyncOnShim()
        {
            _serviceBusShim.GetNumberOfActiveSubscriptions(Topic).Returns(1);

            await _messageService.Stop();

            await _serviceBusShim.Received(1).CloseTopicClientAsync();
            await _serviceBusShim.DidNotReceive().DeleteTopicAsync(Topic);
        }

        [Test]
        public async Task MessageService_StopWithNoSubscriptions_CallsCloseTopicClientAsyncOnShim()
        {
            _serviceBusShim.GetNumberOfActiveSubscriptions(Topic).Returns(0);

            await _messageService.Stop();

            Received.InOrder(() =>
            {
                _serviceBusShim.Received(1).CloseTopicClientAsync();
                _serviceBusShim.Received(1).DeleteTopicAsync(Topic);
            });
        }

    }
}
