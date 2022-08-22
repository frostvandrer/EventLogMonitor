using System;

namespace EventLogMonitor
{
    internal class LogEntry
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public int RecordID { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Message { get; set; }

        public LogEntry() { }

        public LogEntry(int eventID, string eventName, int recordID, DateTime? timestamp, string message)
        {
            EventID = eventID;
            EventName = eventName;
            RecordID = recordID;
            Timestamp = timestamp;
            Message = message;
        }
    }
}
