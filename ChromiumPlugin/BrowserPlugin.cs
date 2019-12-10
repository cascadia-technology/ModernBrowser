using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using CefSharp.Callback;
using ModernBrowserShim.Properties;

namespace ModernBrowserShim
{
    public static class BrowserPlugin
    {
        private static readonly object InitLock = new object();
        private static bool _initialized;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Initialize(string cachePath)
        {
            lock (InitLock)
            {
                if (_initialized) return;
                if (Cef.IsInitialized) return;
                var browserSubProcessPath = Path.Combine
                (
                    new FileInfo(typeof(BrowserPlugin).Assembly.Location).DirectoryName,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    "CefSharp.BrowserSubprocess.exe"
                );
                var settings = new CefSettings
                {
                    BrowserSubprocessPath = browserSubProcessPath, 
                    CachePath = cachePath, 
                    BackgroundColor = 0xFF,
                    UserAgent = "ModernBrowser/1.0 (Windows; Win64; x64) Chromium",
                    LogFile = Path.Combine(cachePath, "debug.log")
                };
                settings.RegisterScheme(new CefCustomScheme {
                    SchemeName = "help",
                    SchemeHandlerFactory = new AboutSchemeHandlerFactory()
                });
                Application.Current.Dispatcher?.Invoke(() =>
                    Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null));
                _initialized = true;
            }
        }
    }

    public class AboutSchemeHandlerFactory : ISchemeHandlerFactory
    {
        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            if (request.Url.Equals(@"help://script/"))
            {
                return ResourceHandler.FromString(Resources.AboutScript);
            }

            return null;
        }
    }
}
