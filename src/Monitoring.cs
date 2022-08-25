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
        private string popUpMessage = String.Empty;

        private readonly string PowerShellLogName = "Microsoft-Windows-PowerShell/Operational";
        private readonly string SecurityLogName = "Security";

        public EventLogWatcher? PowerShellWatcher { get; set; }
        public EventLogWatcher? SecurityWatcher { get; set; }

        public Monitoring(NotifyIcon trayIcon)
        {
            this.trayIcon = trayIcon;
            MonitorPowershell();
            MonitorSecurity();
        }

        public async void MonitorPowershell()
        {
            await Task.Run(() =>
            {
                try
                {
                    EventLogQuery query = new EventLogQuery(PowerShellLogName, PathType.LogName);
                    
                    PowerShellWatcher = new EventLogWatcher(query);
                    PowerShellWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(SuspiciousEventHandler);
                    PowerShellWatcher.Enabled = true;

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
                    // Stop listening to events
                    PowerShellWatcher.Enabled = false;

                    if (PowerShellWatcher != null)
                    {
                        PowerShellWatcher.Dispose();
                    }
                }
            });
        }

        public async Task MonitorSecurity()
        {
            await Task.Run(() =>
            {
                try
                {
                    EventLogQuery query = new EventLogQuery(SecurityLogName, PathType.LogName);

                    SecurityWatcher = new EventLogWatcher(query);
                    SecurityWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(SuspiciousEventHandler);
                    SecurityWatcher.Enabled = true;

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
                    // Stop listening to events
                    SecurityWatcher.Enabled = false;

                    if (SecurityWatcher != null)
                    {
                        SecurityWatcher.Dispose();
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
                if (PowerShellEvent.IsSuspicious(e))
                {
                    SuspiciousActivityDetected();
                }
            }
            else if (e.EventRecord.LogName == SecurityLogName)
            {
                if (SecurityEvent.IsSuspicious(e))
                {
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
