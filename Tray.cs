using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EventLogMonitor
{
    internal class Tray : ApplicationContext
    {
        private NotifyIcon TrayIcon { get; set; }
        Monitoring monitor;

        ContextMenuStrip menu;

        public Tray()
        {
            TrayIcon = new NotifyIcon();
            menu = new ContextMenuStrip();

            TrayIcon.Icon = new System.Drawing.Icon("..\\..\\..\\Resources\\app-white.ico");
            TrayIcon.ContextMenuStrip = menu;

            ToolStripMenuItem readLog = new ToolStripMenuItem("Start Monitoring");
            readLog.Click += new EventHandler(StartMonitoring);

            ToolStripMenuItem pauseMonitoring = new ToolStripMenuItem("Pause Monitoring");
            pauseMonitoring.Click += new EventHandler(PauseMonitoring);

            ToolStripMenuItem exitMenu = new ToolStripMenuItem("Exit");
            exitMenu.Click += new EventHandler(Exit);

            menu.Items.Add(readLog);
            menu.Items.Add(pauseMonitoring);
            menu.Items.Add(exitMenu);

            TrayIcon.Visible = true;

            monitor = new Monitoring(TrayIcon);
        }

        void StartMonitoring(object sender, EventArgs e)
        {
             monitor = new Monitoring(TrayIcon);
        }

        void PauseMonitoring(object sender, EventArgs e)
        {
            monitor.Watcher.Enabled = false;
        }

        void Exit(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
