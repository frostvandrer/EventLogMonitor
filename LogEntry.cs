using System;
using System.Diagnostics.Eventing.Reader;

namespace EventLogMonitor
{
    internal class LogEntry
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public long? RecordID { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Message { get; set; }

        public LogEntry(EventRecord r)
        {
            EventID = r.Id;
            EventName = r.TaskDisplayName;
            RecordID = r.RecordId;
            Timestamp = r.TimeCreated;
            Message = r.FormatDescription();
        }

        public static LogEntry? CreateObj(EventRecord r)
        {
            if (r == null)
            {
                return null;
            }

            return new LogEntry(r);
        }
    }
}
