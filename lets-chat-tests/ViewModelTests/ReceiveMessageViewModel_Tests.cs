using lets_chat;
using lets_chat.ViewModels;
using NSubstitute;
using NUnit.Framework;
using System;

namespace lets_chat_tests.ViewModelTests
{
    [TestFixture]
    public class ReceiveMessageViewModel_Tests
    {
        private IMessageService _messageService;
        private IReceiveMessageViewModel _viewModel;

        [SetUp]
        public void TestInit()
        {
            _messageService = Substitute.For<IMessageService>();
            _viewModel = new ReceiveMessageViewModel(_messageService);
        }

        [Test]
        public void ReceiveMessageViewModel_OnMessageServiceRaisesReceivedMessage_SetsMessageToReceivedMessage()
        {
            const string ExpectedMsg = "Test Message";

            _messageService.MessageReceived += Raise.Event<EventHandler<string>>(this, ExpectedMsg);

            Assert.That(_viewModel.Message, Is.EqualTo(ExpectedMsg));
        }
    }
}
