namespace ModernBrowser
{
    public partial class BrowserPropertiesWpfUserControl
    {
        public BrowserPropertiesWpfUserControl(BrowserViewItemManager vim)
        {
            InitializeComponent();
            DataContext = vim.Settings;
        }
    }
}
