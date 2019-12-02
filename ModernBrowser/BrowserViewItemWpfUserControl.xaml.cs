using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CascadiaControls;
using ChromiumPlugin;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace ModernBrowser
{
    public partial class BrowserViewItemWpfUserControl
    {
        private readonly List<object> _filters = new List<object>();
        private BrowserHost _browser;
        public override bool Selectable => true;
        public override bool ShowToolbar => false;

        public BrowserViewItemWpfUserControl(BrowserSettings settings)
        {
            InitializeComponent();
            DataContext = settings;
        }

        public override void Init()
        {
            try
            {
                _filters.Add(EnvironmentManager.Instance.RegisterReceiver
                (
                    ModeChanged,
                    new MessageIdFilter(MessageId.SmartClient.WorkSpaceStateChangedIndication)
                ));
                var settings = (BrowserSettings)DataContext;
                _browser = new BrowserHost(settings.Address);
                Content = _browser;
            }
            catch (Exception ex)
            {
                MessageBar.Error(ex.Message);
            }
        }

        private object ModeChanged(Message message, FQID destination, FQID sender)
        {
            if (!(message.Data is WorkSpaceState state)) return null;
            switch (state)
            {
                case WorkSpaceState.Normal:
                    Content = _browser;
                    break;
                case WorkSpaceState.Setup:
                    var grid = new Grid();
                    grid.Children.Add(new TextBlock
                    {
                        Text = "Setup Mode", 
                        HorizontalAlignment = HorizontalAlignment.Center, 
                        VerticalAlignment = VerticalAlignment.Center,
                        Background = Brushes.Transparent
                    });
                    Content = new BrowserViewItemSetupWpfUserControl(DataContext as BrowserSettings);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        private void BrowserViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireClickEvent();
            }
        }

        private void BrowserViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireDoubleClickEvent();
            }
        }

        public override void Close()
        {
            _filters.ForEach(EnvironmentManager.Instance.UnRegisterReceiver);
            _browser.Dispose();
        }
    }
}
