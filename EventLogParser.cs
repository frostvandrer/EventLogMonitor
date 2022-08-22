using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace EventLogMonitor
{
    internal static class EventLogParser
    {          
        public static LogEntry? ReadLog(EventLogQuery query, int suspiciousEventID, int recordID)
        {
            query.ReverseDirection = true;

            using (EventLogReader reader = new EventLogReader(query))
            {
                for (EventRecord rec = reader.ReadEvent(); rec != null; rec = reader.ReadEvent())
                {
                    if (rec.RecordId == recordID)
                    {
                        break;
                    }

                    if (rec.Id == suspiciousEventID)
                    {
                        return new LogEntry(rec.Id, rec.TaskDisplayName, (int)rec.RecordId, rec.TimeCreated, rec.FormatDescription());
                    }
                }
            }

            return null;
        }
    }
}
