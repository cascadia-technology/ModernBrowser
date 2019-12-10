using ModernBrowser.Annotations;
using ModernBrowser.SmartClientScripting;
using ModernBrowserShim;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CascadiaControls;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace ModernBrowser
{
    public partial class BrowserViewItemWpfUserControl : INotifyPropertyChanged
    {
        private readonly List<object> _filters = new List<object>();
        private readonly BrowserViewItemManager _vim;
        private BrowserHost _browser;
        public override bool Selectable => true;
        public override bool ShowToolbar => false;

        public BrowserViewItemWpfUserControl()
        {
            
        }
        public BrowserViewItemWpfUserControl(BrowserViewItemManager vim)
        {
            _vim = vim;
            InitializeComponent();
            NavigationBar.Visibility = _vim.Settings.ShowNavigationBar ? Visibility.Visible : Visibility.Collapsed;
        }

        public override void Init()
        {
            try
            {
                _filters.Add(EnvironmentManager.Instance.RegisterReceiver(ThemeChangedHandler, new MessageIdFilter(MessageId.SmartClient.ThemeChangedIndication)));
                ApplyTheme(ClientControl.Instance.Theme);
                _vim.Settings.PropertyChanged += SettingsOnPropertyChanged;
                _browser = new BrowserHost(_vim.Settings.Address, _vim.Settings.DpiScaleFactor / 100);
                DataContext = _browser;
                UpdateBrowserSettings();
                Panel.Children.Add(_browser);
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionHandler(GetType().Name, nameof(Init), ex);
            }
        }

        private object ThemeChangedHandler(Message message, FQID destination, FQID sender)
        {
            ApplyTheme(ClientControl.Instance.Theme);
            return null;
        }

        private void ApplyTheme(Theme theme)
        {
            NavBack.Foreground = GetBrush(theme.TextColor);
            NavForward.Foreground = GetBrush(theme.TextColor);
            NavRefresh.Foreground = GetBrush(theme.TextColor);
            NavHome.Foreground = GetBrush(theme.TextColor);
            NavPrint.Foreground = GetBrush(theme.TextColor);
        }
        private static Brush GetBrush(System.Drawing.Color color)
        {
            return new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                UpdateBrowserSettings();
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionHandler(GetType().Name, nameof(UpdateBrowserSettings), ex);
            }
        }

        private void UpdateBrowserSettings()
        {
            if (_vim.Settings.EnableSmartClientScripting)
            {
                _browser.RegisterJsObject("SCSApplication", new SCSApplicationMethods());
                _browser.RegisterJsObject("SCSGeneral", new SCSGeneralMethods());
            }
            else
            {
                _browser.UnregisterJsObject("SCSApplication");
                _browser.UnregisterJsObject("SCSGeneral");
            }
            _browser.SetHome(_vim.Settings.Address);
            _browser.GoHome();
            NavigationBar.Visibility = _vim.Settings.ShowNavigationBar ? Visibility.Visible : Visibility.Collapsed;
            _browser.DpiScaleFactor = _vim.Settings.DpiScaleFactor / 100;
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
            _vim.Settings.PropertyChanged -= SettingsOnPropertyChanged;
            _filters.ForEach(EnvironmentManager.Instance.UnRegisterReceiver);
            _browser?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            _browser.GoHome();
        }

        private void BrowserViewItemWpfUserControl_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                _browser.ShowDevTools();
            }
        }
    }
}
