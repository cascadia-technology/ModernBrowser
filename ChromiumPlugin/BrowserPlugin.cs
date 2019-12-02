using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using VideoOS.Platform.Client;

namespace ChromiumPlugin
{
    public static class BrowserPlugin
    {
        private static readonly object InitLock = new object();
        private static bool _initialized;

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Initialize()
        {
            lock (InitLock)
            {
                if (_initialized) return;
                var browserSubProcessPath = Path.Combine
                (
                    new FileInfo(typeof(BrowserPlugin).Assembly.Location).DirectoryName,
                    Environment.Is64BitProcess ? "x64" : "x86",
                    "CefSharp.BrowserSubprocess.exe"
                );
                var settings = new CefSettings { BrowserSubprocessPath = browserSubProcessPath };
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
                _initialized = true;
            }
        }
    }
}
