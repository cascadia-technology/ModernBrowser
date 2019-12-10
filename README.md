# ModernBrowser

This is a MIP Plugin for Milestone XProtect Smart Client which adds a Chromium browser view item
plugin allowing you to display just about any web content that Chrome is capable of displaying.

The embedded browser is made possible by CefSharp, and the implementation in Smart Client supports
the following

- Simple navigation bar which can be enabled/disabled in the Properties pane for each browser instance
- Limited support for Smart Client Scripting with a very small number of methods implemented
- Adjustable zoom level from 25% to 500%
- Caching to improve the user experience when Smart Client is re-opened
