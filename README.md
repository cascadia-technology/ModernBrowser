# ModernBrowser

This is a MIP Plugin for Milestone XProtect Smart Client which adds a Chromium browser view item
plugin allowing you to display just about any web content that Chrome is capable of displaying.

The embedded browser is made possible by CefSharp, and the implementation in Smart Client supports
the following

- Simple navigation bar which can be enabled/disabled in the Properties pane for each browser instance
- Limited support for Smart Client Scripting with a very small number of methods implemented
    - Set Address to help:script to display the same Smart Client Scripting sample page available in the
    built-in Smart Client HTML Page view item when you use the address 'about:script'
    - SCS.Application.Maximize()
    - SCS.Application.Minimize()
    - SCS.Application.Restore()
    - SCS.Application.Close()
    - SCS.Application.ReloadConfiguration()
    - SCS.General.ActivateEvent(eventName)    
- Adjustable zoom level from 25% to 500%
- Caching to improve the user experience when Smart Client is re-opened
    - Cache and debug.log path for CefSharp set to %AppData%/Modern Browser/
- Support for 32bit and 64bit Smart Client
    - Single installer adds both 32bit and 64bit plugin
    - Not tested on 32bit Windows. The installer may only be compatible with 64bit operating systems but
    the plugin itself should be compatible with 32bit Windows. May need to author a separate installer or
    simply copy/paste the plugin from a 64bit OS into the C:\Program Files\VideoOS\MIPPlugins\ModernBrowser
    path on the 32bit OS. 
