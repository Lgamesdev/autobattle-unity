namespace LGamesDev.Core
{
    public class SocketMessage
    {
        public string Action;
        public string Channel;
        public string Username;
        public string Content;
        
        public override string ToString()
        {
            return "SocketMessage : [ \n" +
                       "Action : " + Action + "\n" +
                       "Channel : " + Channel + "\n" +
                       "Username : " + Username + "\n" +
                       "Content : " + Content + "\n" +
                   "]";
        }
    }
}