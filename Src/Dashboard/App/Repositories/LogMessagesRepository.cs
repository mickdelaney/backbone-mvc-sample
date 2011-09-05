using System.Collections.Generic;
using System.Collections.ObjectModel;
using Dashboard.App.Models;

namespace Dashboard.App.Repositories
{
    public class LogMessagesRepository
    {
        readonly IList<LogMessage> _messages = new List<LogMessage>();

        public ReadOnlyCollection<LogMessage> GetMessages()
        {
            return new ReadOnlyCollection<LogMessage>(_messages);
        }

        public void AddMessage(LogMessage message)
        {
            _messages.Add(message);
        }

    }
}