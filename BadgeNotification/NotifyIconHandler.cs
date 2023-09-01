
using System.Globalization;
using Forms = System.Windows.Forms;

namespace BadgeNotification
{
    public class NotifyIconHandler
    {
        private Forms.NotifyIcon _notifyIcon;
       public void InitializeNotifyIcon()
        {
            _notifyIcon = new Forms.NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("logo.ico");
            _notifyIcon.Visible = true;
        }
        // Public static method to access the single instance of the class     

        public void BadgeHideTooltip()
        {
            _notifyIcon.Icon = new System.Drawing.Icon("logo.ico");
            _notifyIcon.Text = "Badge Hide";

        }
        public void BadgeShowTooltip()
        {
            _notifyIcon.Icon = new System.Drawing.Icon("logo.ico");
            _notifyIcon.Text = "Badge Show";           
        }


        public void ShowBadgeNotification()
        {       
            _notifyIcon.ShowBalloonTip(100, "Badge", "Show Badge", Forms.ToolTipIcon.Info);
        }
        public void HideBadgeNotification()
        {
          _notifyIcon.ShowBalloonTip(100, "Badge", "Hide Badge", Forms.ToolTipIcon.Warning);
        }
        public void DisposeNotifyIcon()
        {
            _notifyIcon.Dispose();
        }
    }
}
