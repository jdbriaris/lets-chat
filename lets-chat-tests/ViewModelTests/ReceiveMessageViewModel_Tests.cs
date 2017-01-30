using GalaSoft.MvvmLight.Threading;
using lets_chat;
using lets_chat.ViewModels;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace lets_chat_tests.ViewModelTests
{
    [TestFixture]
    public class ReceiveMessageViewModel_Tests
    {
        private IMessageService _messageService;
        private IReceiveMessageViewModel _viewModel;

        [OneTimeSetUp]
        public void FixtureInit()
        {
            DispatcherHelper.Initialize();
        }

        [SetUp]
        public void TestInit()
        {
            _messageService = Substitute.For<IMessageService>();
            _viewModel = new ReceiveMessageViewModel(_messageService);
        }

        [Test]
        public void ReceiveMessageViewModel_OnMessageServiceRaisesReceivedMessage_SetsMessageToReceivedMessage()
        {
            const string MessageOne = "Message One";
            const string MessageTwo = "Message Two";
            var expectedMessages = new List<string>
            {
                MessageOne, MessageTwo
            };

            _messageService.MessageReceived += Raise.Event<EventHandler<string>>(this, MessageOne);
            _messageService.MessageReceived += Raise.Event<EventHandler<string>>(this, MessageTwo);

            CollectionAssert.AreEqual(_viewModel.Messages, expectedMessages);
        }
    }
}
