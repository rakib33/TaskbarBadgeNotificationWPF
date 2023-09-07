
using System;
using System.Management;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BadgeNotification
{

    public partial class MainWindow : Window
    {
        NotifyIconHandler notifyIconHandler;
        //http://10rem.net/blog/2010/05/29/creating-dynamic-windows-7-taskbar-overlay-icons
        public MainWindow()
        {
            InitializeComponent();
            string iconOverlayText = "1";
            notifyIconHandler = new NotifyIconHandler();
            notifyIconHandler.InitializeNotifyIcon();

            #region CameraConnectDisconnectEventCatch
            //Windows Management Instrumentation (WMI) 
            // Define the query to monitor for device connection events
            WqlEventQuery query = new WqlEventQuery("SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_PnPEntity'");

            // Create a management event watcher with the query
            ManagementEventWatcher watcher = new ManagementEventWatcher(query);

            // Attach an event handler for the event received
            watcher.EventArrived += DeviceChangeEvent;

            // Start listening for events
            watcher.Start();

            //Console.WriteLine("Monitoring for camera connection and disconnection events. Press Enter to exit.");
            //Console.ReadLine();

            // Stop listening for events when done
            // watcher.Stop();
            #endregion
        }

        private void DeviceChangeEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];

            // Check if the event is for a USB camera
            if (instance.Properties["PNPClass"].Value != null && instance.Properties["PNPClass"].Value.ToString().Contains("Camera"))
            {
                string eventType = e.NewEvent.ClassPath.ClassName;
                string deviceName = instance.Properties["Name"].Value.ToString();

                if (eventType == "__InstanceCreationEvent")
                {
                    Console.WriteLine($"Camera connected: {deviceName}");
                }
                else if (eventType == "__InstanceDeletionEvent")
                {
                    Console.WriteLine($"Camera disconnected: {deviceName}");
                }
            }
        }

        private void UpdateCount_Click(object sender, RoutedEventArgs e)
        {
            int iconWidth = 20;
            int iconHeight = 20;

            string countText = EnteredCount.Text.Trim();

            RenderTargetBitmap bmp =
            new RenderTargetBitmap(iconWidth, iconHeight, 96, 96, PixelFormats.Default);
            ContentControl root = new ContentControl();
            root.ContentTemplate = ((DataTemplate)Resources["OverlayIcon"]);
            root.Content = countText;
            root.Arrange(new Rect(0, 0, iconWidth, iconHeight));
            bmp.Render(root);
            TaskbarItemInfo.Overlay = (ImageSource)bmp;

            //display notification icon
            notifyIconHandler.BadgeShowTooltip();
            notifyIconHandler.ShowBadgeNotification();
        }

        private void RemoveCount_Click(object sender, RoutedEventArgs e)
        {
            TaskbarItemInfo.Overlay = null;
            //hide notify icon
            notifyIconHandler.BadgeHideTooltip();
            notifyIconHandler.HideBadgeNotification();
        }
    }
}
