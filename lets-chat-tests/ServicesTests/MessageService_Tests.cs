using lets_chat;
using lets_chat.Services;
using Microsoft.ServiceBus.Messaging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

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

        //TODO need more tests for TopicClient and SubscriptionClient logic. Need to put these in shim somehow.
    }
}
