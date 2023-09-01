
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
