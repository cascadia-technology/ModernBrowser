using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CascadiaControls;
using ChromiumPlugin;
using ModernBrowser.Annotations;
using VideoOS.Platform;
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

        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (value.Equals(_address)) return;
                _address = value;
                _browser?.GoTo(value);
                OnPropertyChanged();
            }
        }

        public BrowserViewItemWpfUserControl(BrowserViewItemManager vim)
        {
            _vim = vim;
            _address = _vim.Settings.Address;
            InitializeComponent();
            AddressBar.Visibility = _vim.Settings.ShowAddressBar ? Visibility.Visible : Visibility.Collapsed;
            DataContext = this;
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
                _vim.Settings.PropertyChanged += SettingsOnPropertyChanged;
                _browser = new BrowserHost(_vim.Settings.Address);
                Panel.Children.Add(_browser);
            }
            catch (Exception ex)
            {
                EnvironmentManager.Instance.ExceptionHandler(GetType().Name, nameof(Init), ex);
            }
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(_vim.Settings.Address)))
            {
                Address = _vim.Settings.Address;
                _browser.GoTo(_vim.Settings.Address);
            }

            AddressBar.Visibility = _vim.Settings.ShowAddressBar ? Visibility.Visible : Visibility.Collapsed;
        }

        private object ModeChanged(Message message, FQID destination, FQID sender)
        {
            return null;
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
            _browser?.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddressBar_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            Address = ((TextBox) sender).Text;
            Panel.Focus();
        }

        private void Back_OnClick(object sender, RoutedEventArgs e)
        {
            _browser.GoBack();
        }

        private void Forward_OnClick(object sender, RoutedEventArgs e)
        {
            _browser.GoForward();
        }

        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            _browser.Refresh();
        }

        private void Home_OnClick(object sender, RoutedEventArgs e)
        {
            _browser.GoHome();
        }
    }
}
