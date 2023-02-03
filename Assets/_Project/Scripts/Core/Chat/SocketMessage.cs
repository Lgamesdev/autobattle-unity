namespace LGamesDev.Core
{
    public class SocketMessage
    {
        public WebSocketAction Action;
        public string Channel;
        public string User;
        public string Message;

    }

    public enum WebSocketAction
    {
        Subscribe,
        Message
    }
}