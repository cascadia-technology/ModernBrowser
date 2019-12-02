using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using ChromiumPlugin;
using VideoOS.Platform.Client;

namespace ModernBrowser
{
    public class BrowserViewItemPlugin : ViewItemPlugin
    {
        private string _pluginPath;
        public override Guid Id { get; }
        public override string Name { get; }
        public override Image Icon { get; }

        public BrowserViewItemPlugin()
        {
            Id = Identifiers.ModernBrowser;
            Name = "Modern Browser";
            Icon = SystemIcons.Application.ToBitmap();
            _pluginPath = new FileInfo(GetType().Assembly.Location).DirectoryName;
        }
        public override ViewItemManager GenerateViewItemManager()
        {
            return new BrowserViewItemManager(Name);
        }

        public override void Init()
        {
            BrowserPlugin.Initialize();            
        }

        public override void Close()
        {
        }
    }
}