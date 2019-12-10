using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModernBrowserShim
{
    public class BrowserHost : UserControl, IDisposable
    {
        private readonly object _lockObj = new object();
        private ChromiumWebBrowser _browser;
        private string _home;

        public string Address
        {
            get => _browser?.Address;
            set => _browser.Address = value;
        }

        public float DpiScaleFactor
        {
            get => _browser.DpiScaleFactor;
            set
            {
                _browser.DpiScaleFactor = value;
                _browser.NotifyDpiChange(value);
            }
        }

        public ICommand ReloadCommand => _browser?.ReloadCommand;
        public ICommand BackCommand => _browser?.BackCommand;
        public ICommand ForwardCommand => _browser?.ForwardCommand;
        public ICommand PrintCommand => _browser?.PrintCommand;

        public BrowserHost()
        {
            Init();
        }

        public BrowserHost(string initialAddress, float dpiScaleFactor = 1)
        {
            _home = initialAddress;
            Init(initialAddress, dpiScaleFactor);
        }

        private void Init(string initialAddress = null, float scaleFactor = 1)
        {
            _browser = new ChromiumWebBrowser(initialAddress ?? string.Empty) {DpiScaleFactor = scaleFactor};
            _browser.KeyDown += OnKeyDown;
            _browser.PreviewMouseDown += OnMouseDown;
            _browser.FrameLoadEnd += (sender, args) =>
            {
                if (args.Frame.IsMain)
                {
                    DpiScaleFactor = scaleFactor;
                    args.Frame.ExecuteJavaScriptAsync(Properties.Resources.SCSBindScript);
                }
                
            };
            Content = _browser;
        }

        public void GoHome()
        {
            _browser.Address = _home;
        }

        public void SetHome(string address)
        {
            _home = address;
        }

        public void RegisterJsObject(string jsObjectName, object obj)
        {
            lock (_lockObj)
            {
                if (_browser.JavascriptObjectRepository.IsBound(jsObjectName)) return;
                _browser.JavascriptObjectRepository.Register(jsObjectName, obj, false, new BindingOptions { CamelCaseJavascriptNames = false });
            }
        }

        public void UnregisterJsObject(string jsObjectName)
        {
            lock (_lockObj)
            {
                if (!_browser.JavascriptObjectRepository.IsBound(jsObjectName)) return;
                _browser.JavascriptObjectRepository.UnRegister(jsObjectName); 
            }
        }

        public void ShowDevTools()
        {
            _browser.ShowDevTools();
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Mark KeyDownEvent as handled if Key == Enter
            // Otherwise Smart Client will maximize/restore the viewitem
            if (e.Key != Key.Enter) return;
            e.Handled = true;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Re-Raise the MouseDown event so that Smart Client can select the viewitem
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