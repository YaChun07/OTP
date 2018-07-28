using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace RsaSecureToken.Tests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private IProfile _fakeProfile = Substitute.For<IProfile>();
        private IToken _fakeToken = Substitute.For<IToken>();
        private ILogger _fakeLogger = Substitute.For<ILogger>();
        private AuthenticationService _authenticationService;

        [TestInitialize]
        public void SetUp()
        {
            _authenticationService = new AuthenticationService(_fakeProfile, _fakeToken, _fakeLogger);
        }

        [TestMethod]
        public void IsValidTest()
        {
            GetPassword("joey", "91");
            GetRandom("000000");
            ShouldValid("joey", "91000000");
        }

        [TestMethod]
        public void IsInvalid()
        {
            GetPassword("joey", "91");
            GetRandom("000000");
            ShouldInvalid("joey", "Wrong Password");
        }

        [TestMethod]
        public void ShouldCallLogSaveOnce()
        {
            GetPassword("joey", "91");
            GetRandom("000000");
            ShouldInvalid("joey", "Wrong Password");

            _fakeLogger.Received(1);
        }

        [TestMethod]
        public void ShouldCallLogSaveOnce_returnAccount()
        {
            GetPassword("joey", "91");
            GetRandom("000000");
            ShouldInvalid("joey", "Wrong Password");

            _fakeLogger.Received(1).Save(Arg.Is<string>(x => x.Contains("joey") && x.Contains("login failed")));
        }

        [TestMethod]
        public void Should_not_log_when_valid()
        {
            GetPassword("joey", "91");
            GetRandom("000000");
            ShouldValid("joey", "91000000");

            _fakeLogger.DidNotReceiveWithAnyArgs().Save("");
        }

        private void ShouldValid(string account, string password)
        {
            Assert.IsTrue(_authenticationService.IsValid(account, password));
        }

        private void GetRandom(string randomNum)
        {
            _fakeToken.GetRandom("").ReturnsForAnyArgs(randomNum);
        }

        private void GetPassword(string account, string password)
        {
            _fakeProfile.GetPassword(account).Returns(password);
        }

        private void ShouldInvalid(string account, string wrongPassword)
        {
            Assert.IsFalse(_authenticationService.IsValid(account, wrongPassword));
        }
    }
}