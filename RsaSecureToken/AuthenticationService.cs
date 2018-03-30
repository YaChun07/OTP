using System;
using System.Collections.Generic;

namespace RsaSecureToken
{
    public class AuthenticationService
    {
        private readonly IProfile _profile;
        private readonly IToken _token;
        private readonly ILog _log;

        public AuthenticationService()
        {
            _profile = new ProfileDao();
            _token = new RsaTokenDao();
        }

        public AuthenticationService(IProfile profile, IToken token, ILog log)
        {
            _profile = profile;
            _token = token;
            _log = log;
        }

        public bool IsValid(string account, string password)
        {
            // 根據 account 取得自訂密碼
            //IProfile profileDao = new ProfileDao();
            var passwordFromDao = _profile.GetPassword(account);

            // 根據 account 取得 RSA token 目前的亂數
            //IToken rsaToken = new RsaTokenDao();
            var randomCode = _token.GetRandom(account);

            // 驗證傳入的 password 是否等於自訂密碼 + RSA token亂數
            var validPassword = passwordFromDao + randomCode;
            var isValid = password == validPassword;

            _log.Save($"{account}  login failed.");
            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public interface ILog
    {
        void Save(string message);
    }

    public interface IProfile
    {
        string GetPassword(string account);
    }

    public interface IToken
    {
        string GetRandom(string account);
    }

    public class ProfileDao : IProfile
    {
        public string GetPassword(string account)
        {
            return Context.GetPassword(account);
        }
    }

    public static class Context
    {
        public static Dictionary<string, string> profiles;

        static Context()
        {
            profiles = new Dictionary<string, string>();
            profiles.Add("joey", "91");
            profiles.Add("mei", "99");
        }

        public static string GetPassword(string key)
        {
            return profiles[key];
        }
    }

    public class RsaTokenDao : IToken
    {
        public string GetRandom(string account)
        {
            var seed = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            var result = seed.Next(0, 999999).ToString("000000");
            Console.WriteLine("randomCode:{0}", result);

            return result;
        }
    }
}