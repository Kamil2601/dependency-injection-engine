using System;

namespace Factory
{
    public interface INotifier
    {
        string Notify();
    }

    public class Notifier : INotifier
    {
        MessagePart1 part1;
        MessagePart2 part2;

        public Notifier(MessagePart1 part1, MessagePart2 part2)
        {
            this.part1 = part1;
            this.part2 = part2;
        }

        public string Notify()
        {
            return part1.Message() + part2.Message();
        }
    }

    public class MessagePart1
    {
        public string Message()
        {
            return "Message: ";
        }
    }

    public class MessagePart2
    {
        private string message;

        public MessagePart2(string message)
        {
            this.message = message;
        }

        public string Message()
        {
            return message;
        }
    }


    public class NotifierFactory
    {
        private static Func<INotifier> _provider;

        public INotifier CreateNotifier()
        {
            if (_provider != null)
                return _provider();
            else
                throw new ArgumentException();
        }

        public static void SetProvider(Func<INotifier> provider)
        {
            _provider = provider;
        }
    }
}
