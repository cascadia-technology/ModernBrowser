﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.Client;


namespace ModernBrowser
{
    public class BrowserPluginDefinition : PluginDefinition
    {
        private string _pluginPath;
        public override Guid Id { get; }
        public override string Name { get; }
        public override string Manufacturer { get; }
        public override string VersionString { get; }
        public override Image Icon { get; }
        public override List<ViewItemPlugin> ViewItemPlugins { get; }

        public BrowserPluginDefinition()
        {
            Id = Identifiers.PluginId;
            Name = "Plugin";
            Manufacturer = "Cascadia Technology LLC";
            VersionString = GetType().Assembly.GetName().Version.ToString();
            Icon = SystemIcons.Application.ToBitmap();
            ViewItemPlugins = new List<ViewItemPlugin>();
        }

        public override void Init()
        {
            _pluginPath = new FileInfo(GetType().Assembly.Location).DirectoryName;
            AppDomain.CurrentDomain.AssemblyResolve += ResolveCefSharpAssemblies;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveCefSharpAssemblies;
            ViewItemPlugins.Add(new BrowserViewItemPlugin());
        }

        public override void Close()
        {
            ViewItemPlugins.Clear();
            AppDomain.CurrentDomain.AssemblyResolve -= ResolveCefSharpAssemblies;
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ResolveCefSharpAssemblies;
        }

        private Assembly ResolveCefSharpAssemblies(object sender, ResolveEventArgs args)
        {
            if (!args.Name.StartsWith("CefSharp")) return null;

            var dllName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
            var path = Path.Combine
            (
                _pluginPath,
                Environment.Is64BitProcess ? "x64" : "x86",
                dllName);

            return File.Exists(path)
                ? Assembly.LoadFile(path)
                : null;
        }
    }
}
