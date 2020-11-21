using System;

namespace DiscordAPI
{
    public class Wrapper
    {
        public int Test() => 10;

        public void Connect(Action<bool> resultCallback)
        {

        }

        public void GetServers(Action<string[]> resultCallback)
        {

        }

        public void GetTextChannels(Action<string[]> resultCallback)
        {

        }

        public void GetChannelMessages(DateTime from, DateTime to, Action<Message[]> resultCallback)
        {

        }
    }

    public struct Message
    {
        public string Author;
        public DateTime Timestamp;
        public string Text;
    }
}
