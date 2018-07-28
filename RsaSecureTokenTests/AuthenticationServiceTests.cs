using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace RsaSecureToken.Tests
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        [TestMethod]
        public void IsValidTest()
        {
            //var profile = new FakeProfile();
            //var token = new FakeToken();

            var fakeProfile = Substitute.For<IProfile>();
            fakeProfile.GetPassword("joey").Returns("91");

            var fakeToken = Substitute.For<IToken>();
            fakeToken.GetRandom("").ReturnsForAnyArgs("000000");

            var fakeLogger = Substitute.For<ILogger>();
            //var target = new AuthenticationService(profile, token);
            var target = new AuthenticationService(fakeProfile, fakeToken, fakeLogger);

            var actual = target.IsValid("joey", "91000000");

            Assert.IsTrue(actual);
        }


        [TestMethod]
        public void IsInvalid()
        {
            var fakeProfile = Substitute.For<IProfile>();
            fakeProfile.GetPassword("joey").Returns("91");

            var fakeToken = Substitute.For<IToken>();
            fakeToken.GetRandom("").ReturnsForAnyArgs("000000");

            var fakeLogger = Substitute.For<ILogger>();
            var target = new AuthenticationService(fakeProfile, fakeToken, fakeLogger);

            var actual = target.IsValid("joey", "Wrong Password");

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void ShouldCallLogSaveOnce()
        {
            var fakeProfile = Substitute.For<IProfile>();
            fakeProfile.GetPassword("joey").Returns("91");

            var fakeToken = Substitute.For<IToken>();
            fakeToken.GetRandom("").ReturnsForAnyArgs("000000");

            var fakeLogger = Substitute.For<ILogger>();
            var target = new AuthenticationService(fakeProfile, fakeToken, fakeLogger);

            target.IsValid("joey", "Wrong Password");

            fakeLogger.Received(1);
        }

        [TestMethod]
        public void ShouldCallLogSaveOnce_returnAccount()
        {
            var fakeProfile = Substitute.For<IProfile>();
            fakeProfile.GetPassword("joey").Returns("91");

            var fakeToken = Substitute.For<IToken>();
            fakeToken.GetRandom("").ReturnsForAnyArgs("000000");

            var fakeLogger = Substitute.For<ILogger>();
            var target = new AuthenticationService(fakeProfile, fakeToken, fakeLogger);

            target.IsValid("joey", "Wrong Password");

            fakeLogger.Received(1).Save(Arg.Is<string>(x=>x.Contains("joey") && x.Contains("login failed")));
        }

        [TestMethod]
        public void Should_not_log_when_valid()
        {
            var fakeProfile = Substitute.For<IProfile>();
            fakeProfile.GetPassword("joey").Returns("91");

            var fakeToken = Substitute.For<IToken>();
            fakeToken.GetRandom("").ReturnsForAnyArgs("000000");

            var fakeLogger = Substitute.For<ILogger>();
            var target = new AuthenticationService(fakeProfile, fakeToken, fakeLogger);

            target.IsValid("joey", "91000000");
            fakeLogger.DidNotReceiveWithAnyArgs().Save("");
        }
      
            
        
    }

    //public class FakeProfile : IProfile
    //{
    //    public string GetPassword(string account)
    //    {
    //        if (account == "joey")
    //        {
    //            return "91";
    //        }
    //        return "";
    //    }
    //}

    //public class FakeToken : IToken
    //{
    //    public string GetRandom(string account)
    //    {
    //        return "000000";
    //    }
    //}
}
