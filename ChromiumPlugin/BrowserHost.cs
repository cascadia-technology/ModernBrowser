using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Wpf;

namespace ChromiumPlugin
{
    public class BrowserHost : UserControl, IDisposable
    {
        private ChromiumWebBrowser _browser;

        public BrowserHost()
        {
            Init();
        }
        public BrowserHost(string initialAddress)
        {
            Init(initialAddress);
        }

        private void Init(string initialAddress = null)
        {
            _browser = new ChromiumWebBrowser(initialAddress ?? "https://www.cascadia.tech");
            _browser.PreviewMouseDown += BrowserOnMouseDown;
            Content = _browser;
        }

        private void BrowserOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var args = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice)
            {
                RoutedEvent = MouseDownEvent,
                Source = sender
            };
            RaiseEvent(args);
        }

        public void Dispose()
        {
            _browser?.Dispose();
        }
    }
}