using System.Diagnostics.Eventing.Reader;

namespace EventLogMonitor.Events
{
    internal class SecurityEvent : BaseEvent
    {
        public SecurityEvent() { }

        public override bool IsSuspicious(EventRecordWrittenEventArgs e)
        {
            LogEntry? entry = LogEntry.CreateObj(e.EventRecord);

            if (entry == null)
            {
                return false;
            }

            if (AccountCreated(entry.EventID))
            {
                return true;
            }

            return false;
        }

        private static bool AccountCreated(int eventID)
        {
            int suspiciousEventID = 4720;

            return eventID == suspiciousEventID;
        }
    }
}
