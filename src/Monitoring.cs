using System;
using System.Windows.Forms;
using System.Diagnostics.Eventing.Reader;
using EventLogMonitor.Events;
using System.Threading.Tasks;

namespace EventLogMonitor
{
    internal class Monitoring
    {
        private NotifyIcon trayIcon;
        private string popUpMessage = string.Empty;

        private readonly string PowerShellLogName = "Microsoft-Windows-PowerShell/Operational";
        private readonly string SecurityLogName = "Security";

        public EventLogWatcher? PowerShellWatcher { get; set; }
        public EventLogWatcher? SecurityWatcher { get; set; }

        public Monitoring(NotifyIcon trayIcon)
        {
            this.trayIcon = trayIcon;

            _ = MonitorLog(PowerShellWatcher, PowerShellLogName);
            _ = MonitorLog(SecurityWatcher, SecurityLogName);
        }

        public async Task MonitorLog(EventLogWatcher? w, string logName)
        {
            await Task.Run(() =>
            {
                try
                {
                    EventLogQuery query = new EventLogQuery(logName, PathType.LogName);

                    w = new EventLogWatcher(query);
                    w.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(SuspiciousEventHandler);
                    w.Enabled = true;

                    for (; ; )
                    {
                        // Wait for events to occur. 
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (EventLogReadingException e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    if (w != null)
                    {
                        // Stop listening to events
                        w.Enabled = false;

                        if (w != null)
                        {
                            w.Dispose();
                        }
                    }
                }
            });
        }

        private void SuspiciousEventHandler(object? sender, EventRecordWrittenEventArgs e)
        {
            if (e.EventRecord == null)
            {
                return;
            }

            if (e.EventRecord.LogName == PowerShellLogName)
            {
                PowerShellEvent pe = new PowerShellEvent();

                if (pe.IsSuspicious(e))
                {
                    popUpMessage = e.EventRecord.FormatDescription();
                    SuspiciousActivityDetected();
                }
            }
            else if (e.EventRecord.LogName == SecurityLogName)
            {
                SecurityEvent se = new SecurityEvent();

                if (se.IsSuspicious(e))
                {
                    popUpMessage = e.EventRecord.FormatDescription();
                    SuspiciousActivityDetected();
                }
            }
        }

        public void SuspiciousActivityDetected()
        {
            trayIcon.ShowBalloonTip(3000, "Suspicious behaviour detected", "Click here to show more information", ToolTipIcon.Warning);
            trayIcon.BalloonTipClicked += new EventHandler(PopMessageBox);
        }

        private void PopMessageBox(object? sender, EventArgs e)
        {
            MessageBox.Show(popUpMessage);
            trayIcon.BalloonTipClicked -= PopMessageBox;
        }
    }
}
