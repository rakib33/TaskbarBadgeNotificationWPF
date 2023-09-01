# TaskbarBadgeNotificationWPF

This is wpf project to display badge on taskbar icon over .Code behind code also please check this project for MVVM approach https://github.com/thoemmi/TaskbarItemOverlay

1.Create a project Name BadgeNotification
2.Create a round circle 
3.Check this xmle code

<Window x:Class="BadgeNotification.MainWindow" 
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation" 
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" 
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
        xmlns:local="clr-namespace:BadgeNotification" 
        mc:Ignorable="d" 
        Title="MainWindow" Height="450" Width="800">
    <Window.TaskbarItemInfo>
        <TaskbarItemInfo/>
    </Window.TaskbarItemInfo>
    <Window.Resources>
        <DataTemplate x:Key="OverlayIcon">
            <Grid Width="20" Height="20">
                <Ellipse Fill="Red" 
                        Stroke="White" 
                        StrokeThickness="2"/>

                <TextBlock Text="{Binding}" 
                        TextAlignment="Center" 
                        Foreground="White" 
                        FontWeight="Bold" 
                        Height="16" 
                        VerticalAlignment="Center" 
                        FontSize="12">
                    <TextBlock.Effect>
                        <DropShadowEffect ShadowDepth="0" />
                    </TextBlock.Effect>
                </TextBlock>
            </Grid>
        </DataTemplate>
    </Window.Resources>
    <Grid>
        <TextBlock Height="23" 
                HorizontalAlignment="Left" 
                Margin="22,71,0,0" 
                x:Name="textBlock1" 
                Text="Count" 
                VerticalAlignment="Top" />
        <TextBox Height="23" 
                HorizontalAlignment="Left" 
                Margin="92,68,0,0" 
                x:Name="EnteredCount" 
                VerticalAlignment="Top" 
                Width="120" />
        <Button Content="Update" 
            Height="23" 
            HorizontalAlignment="Left" 
            Margin="92,105,0,0" 
            x:Name="UpdateCount" 
            VerticalAlignment="Top" 
            Width="75" Click="UpdateCount_Click" />
        <Button Content="Remove"
            Height="23"
            HorizontalAlignment="Left"
            Margin="92,135,0,0"
            x:Name="RemoveCount"
            VerticalAlignment="Top"
            Width="75" Click="RemoveCount_Click" />
    </Grid>
</Window>

4. In code behind
   
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

5.For Task Tray NotifyIconHandler create a clas named NotifyIconHandler.cs
6.Apply this code


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
