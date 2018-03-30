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
            var profile = Substitute.For<IProfile>();
            profile.GetPassword("").ReturnsForAnyArgs("91");

            var token = Substitute.For<IToken>();
            token.GetRandom("").ReturnsForAnyArgs("000000");

            var log = Substitute.For<ILog>();

            var target = new AuthenticationService(profile, token, log);

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);
        }


        [TestMethod()]
        public void Invalid_should_log()
        {
            var profile = Substitute.For<IProfile>();
            profile.GetPassword("").ReturnsForAnyArgs("91");

            var token = Substitute.For<IToken>();
            token.GetRandom("").ReturnsForAnyArgs("000000");

            var log = Substitute.For<ILog>();

            var target = new AuthenticationService(profile, token, log);

            var actual = target.IsValid("joey", "wrong password");
            log.Received(1).Save(Arg.Is<string>(s => s.Contains("joey")));
            //log.Received(1).Save("joey login failed");
        }
    }
}