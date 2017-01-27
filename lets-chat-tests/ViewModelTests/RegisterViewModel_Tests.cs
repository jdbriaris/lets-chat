using lets_chat;
using lets_chat.ViewModels;
using NSubstitute;
using NUnit.Framework;
using System;

namespace lets_chat_tests.ViewModelTests
{
    [TestFixture]
    public class RegisterViewModel_Tests
    {
        private const string EnterButtonText = "Enter Chat Room";
        private const string LeaveButtonText = "Leave Chat Room";
        private TestEnvironment _env;

        [SetUp]
        public void TestInit()
        {
            _env = new TestEnvironment();
        }

        [Test]
        public void RegisterViewModel_OnConstruction_SetsTextOnButtonToEnter()
        {
            Assert.That(_env.ViewModel.ButtonText, Is.EqualTo(EnterButtonText));
        }

        [Test]
        public void RegisterViewModel_OnConstruction_EnterNameTextBoxIsEnabled()
        {
            Assert.That(_env.ViewModel.IsEnterNameTextBoxEnabled, Is.True);
        }

        [Test]
        public void RegisterViewModel_UserEntersChatRoom_CallsRegisterUserOnMessageService()
        {
            const string UserName = "Bob";
            _env.UserEntersChatRoomWithName(UserName);

            _env.MessageService.Received(1).RegisterUser(UserName);
        }

        [Test]
        public void RegisterViewModel_UserEntersChatRoom_SetsTextOnButtonToLeave()
        {
            _env.UserEntersChatRoomWithName("Bill");

            Assert.That(_env.ViewModel.ButtonText, Is.EqualTo(LeaveButtonText));
        }

        [Test]
        public void RegisterViewModel_UserEntersChatRoom_DisablesEnterNameTextBox()
        {
            _env.UserEntersChatRoomWithName("Bill");

            Assert.That(_env.ViewModel.IsEnterNameTextBoxEnabled, Is.False);
        }

        [Test]
        public void RegisterViewModel_UserEntersThenLeavesChatRoom_CallsUnregisterUserOnMessageService()
        {
            _env.UserEntersChatRoomWithName("Jim").UserLeavesChatRoom();

            _env.MessageService.Received(1).UnregisterUser();
        }

        [Test]
        public void RegisterViewModel_UserEntersThenLeavesChatRoom_SetsTextOnButtonToEnter()
        {
            _env.UserEntersChatRoomWithName("Jim").UserLeavesChatRoom();

            Assert.That(_env.ViewModel.ButtonText, Is.EqualTo(EnterButtonText));
        }

        [Test]
        public void RegisterViewModel_UserEntersThenLeavesChatRoom_EnablesEnterNameTextBox()
        {
            _env.UserEntersChatRoomWithName("Jim").UserLeavesChatRoom();

            Assert.That(_env.ViewModel.IsEnterNameTextBoxEnabled, Is.True);
        }

        [Test]
        public void RegisterViewModel_UserEntersThenLeavesChatRoom_SetsUserNameToEmpty()
        {
            _env.UserEntersChatRoomWithName("Jim").UserLeavesChatRoom();

            Assert.That(_env.ViewModel.UserName, Is.Empty);
        }

        #region Test Helpers
        private class TestEnvironment
        {
            public TestEnvironment()
            {
                MessageService = Substitute.For<IMessageService>();
                ViewModel = new RegisterViewModel(MessageService);
            }

            public IRegisterViewModel ViewModel { get; private set; }
            public IMessageService MessageService { get; private set; }

            public TestEnvironment UserEntersChatRoomWithName(string name)
            {
                ViewModel.UserName = name;
                ViewModel.EnterLeaveChatRoomCommand.Execute(null);
                MessageService.UserRegistered += Raise.Event();
                return this;
            }

            public TestEnvironment UserLeavesChatRoom()
            {
                if (string.IsNullOrEmpty(ViewModel.UserName))
                {
                    throw new InvalidOperationException("User must enter chat room before leaving");
                }
                ViewModel.EnterLeaveChatRoomCommand.Execute(null);
                MessageService.UserUnregistered += Raise.Event();
                return this;
            }
        }

        #endregion

    }


}
