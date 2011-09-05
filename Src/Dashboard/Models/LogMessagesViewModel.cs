using System.Collections.ObjectModel;
using Dashboard.App.Models;

namespace Dashboard.Models
{
    public class LogMessagesViewModel
    {
        public ReadOnlyCollection<LogMessage> Messages { get; set; }

        public LogMessagesViewModel(ReadOnlyCollection<LogMessage> messages)
        {
            Messages = messages;
        }
    }
}