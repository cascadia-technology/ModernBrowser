using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp;
using CefSharp.Wpf;

namespace ChromiumPlugin
{
    public class BrowserHost : UserControl, IDisposable
    {
        private ChromiumWebBrowser _browser;
        private string _home;

        public BrowserHost()
        {
            Init();
        }
        public BrowserHost(string initialAddress)
        {
            _home = initialAddress;
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

        public void GoTo(string address)
        {
            _browser.Address = address;
        }

        public void GoBack()
        {
            var webBrowser = _browser.GetBrowser();
            if (webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
            }
        }

        public void GoForward()
        {
            var webBrowser = _browser.GetBrowser();
            if (webBrowser.CanGoForward)
            {
                webBrowser.GoForward();
            }
        }

        public void GoHome()
        {
            _browser.Address = _home;
        }

        public void Refresh()
        {
            _browser.GetBrowser().Reload();
        }
    }
}