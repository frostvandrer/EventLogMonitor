using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace EventLogMonitor
{
    internal class EventLogParser
    {
        private EventLogReader Reader { get; set; }

        public EventLogParser(string logName)
        {
            EventLogQuery query = new EventLogQuery(logName, PathType.LogName);
            query.ReverseDirection = true;

            Reader = new EventLogReader(query);
        }
           
        public string ReadLog()
        {
            StringBuilder sb = new StringBuilder();
            
            using(Reader)
            {
                for (EventRecord rec = Reader.ReadEvent(); rec != null; rec = Reader.ReadEvent())
                {
                    bool happenedToday = rec.TimeCreated > DateTime.Now.AddDays(-1);

                    if (rec.Id == 4104 && happenedToday)
                    {
                        sb.Append($"Event ID: {rec.Id}\n");
                        sb.Append($"Event Name: {rec.TaskDisplayName}\n");
                        sb.Append($"Timestamp: {rec.TimeCreated}\n\n");
                    }
                }
            }
                    
            return sb.ToString();
        }
    }
}
