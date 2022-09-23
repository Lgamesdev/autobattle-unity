using System;
using Core.Player;

namespace LGamesDev.Core.Request
{
    [Serializable]
    public class Authentication
    {
        public string user;

        public string token;
        public string refresh_token;
        public string refresh_token_expiration;
        public PlayerConfig playerConf;
    }
}