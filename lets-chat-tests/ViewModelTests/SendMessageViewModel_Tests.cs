using lets_chat;
using lets_chat.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace lets_chat_tests
{
    [TestFixture]
    public class SendMessageViewModel_Tests
    {
        private IMessageService _messageService;
        private ISendMessageViewModel _viewModel;

        [SetUp]
        public void TestInit()
        {
            _messageService = Substitute.For<IMessageService>();
            _viewModel = new SendMessageViewModel(_messageService);
        }

        [Test]
        public void SendMessageViewModel_OnConstruction_DisablesSendMessage()
        {
            Assert.That(_viewModel.IsSendMessageEnabled, Is.False);
        }

        [Test]
        public void SendMessageViewModel_MessageServiceRaisesUserRegistered_EnablessSendMessage()
        {
            _messageService.UserRegistered += Raise.Event();
            Assert.That(_viewModel.IsSendMessageEnabled, Is.True);
        }

        [Test]
        public void SendMessageViewModel_OnSendMessageCommand_CallsSendMessageOnMessageService()
        {
            const string ExpectedMsg = "Test";
            _viewModel.SendMessageCommand.Execute(ExpectedMsg);
            _messageService.Received(1).SendMessage(ExpectedMsg);
        }
    }

}
