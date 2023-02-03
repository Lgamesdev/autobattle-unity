using System;
using Core.Player;

namespace LGamesDev.Core
{
    [Serializable]
    public class Authentication
    {
        public string username;

        public string token;
        public string refresh_token;
        public string refresh_token_expiration;
        public PlayerConfig PlayerConf;

        public override string ToString()
        {
            return "username : " + username + "\n"
                   + "token : " + token + "\n"
                   + "refresh_token : " + refresh_token + "\n"
                   + "refresh_token_expiration : " + refresh_token_expiration + "\n"
                   + "playerConf : " + PlayerConf.ToString();
        }
    }
}