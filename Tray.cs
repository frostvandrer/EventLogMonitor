using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EventLogMonitor
{
    internal class Tray : ApplicationContext
    {
        private NotifyIcon trayIcon;
        ContextMenuStrip menu;

        public Tray()
        {
            trayIcon = new NotifyIcon();
            menu = new ContextMenuStrip();

            trayIcon.Icon = new System.Drawing.Icon("C:\\Users\\patri\\Documents\\Obsidian\\htb.ico");
            trayIcon.ContextMenuStrip = menu;

            ToolStripMenuItem readLog = new ToolStripMenuItem("Read log");
            readLog.Click += new EventHandler(ShowLastLog);

            ToolStripMenuItem exitMenu = new ToolStripMenuItem("Exit");
            exitMenu.Click += new EventHandler(Exit);

            menu.Items.Add(readLog);
            menu.Items.Add(exitMenu);

            trayIcon.Visible = true;
        }

        void ShowLastLog(object sender, EventArgs e)
        {
            EventLogParser parser = new EventLogParser("Microsoft-Windows-PowerShell/Operational");
            MessageBox.Show(parser.ReadLog());
        }

        void Exit(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
