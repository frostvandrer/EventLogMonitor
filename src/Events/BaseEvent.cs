using System.Diagnostics.Eventing.Reader;

namespace EventLogMonitor.Events
{
    internal abstract class BaseEvent
    {
        public abstract bool IsSuspicious(EventRecordWrittenEventArgs e);
    }
}
