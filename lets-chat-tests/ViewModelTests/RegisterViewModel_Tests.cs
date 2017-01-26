using lets_chat;
using lets_chat.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace lets_chat_tests.ViewModelTests
{
    [TestFixture]
    public class RegisterViewModel_Tests
    {
        private IMessageService _messageService;
        private IRegisterViewModel _viewModel;

        [SetUp]
        public void TestInit()
        {
            _messageService = Substitute.For<IMessageService>();
            _viewModel = new RegisterViewModel(_messageService);
        }

        [Test]
        public void RegisterViewModel_OnRegisterUserCommand_CallsRegisterUserOnMessageService()
        {
            const string ExpectedUser = "Bob";
            _viewModel.RegisterUserCommand.Execute(ExpectedUser);
            _messageService.Received(1).RegisterUser(ExpectedUser);
        }
    }
}
