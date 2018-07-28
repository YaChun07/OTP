using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RsaSecureToken;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace RsaSecureToken.Tests
{
    [TestClass()]
    public class AuthenticationServiceTests
    {
        [TestMethod()]
        public void IsValidTest()
        {
            //var profile = new FakeProfile();
            //var token = new FakeToken();

            var fakeProfile = Substitute.For<IProfile>();
            fakeProfile.GetPassword("joey").Returns("91");

            var fakeToken = Substitute.For<IToken>();
            fakeToken.GetRandom("").ReturnsForAnyArgs("000000");

            //var target = new AuthenticationService(profile, token);
            var target = new AuthenticationService(fakeProfile, fakeToken);

            var actual = target.IsValid("joey", "91000000");

            Assert.IsTrue(actual);
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
