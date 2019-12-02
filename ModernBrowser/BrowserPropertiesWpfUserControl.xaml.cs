namespace ModernBrowser
{
    public partial class BrowserPropertiesWpfUserControl
    {
        public BrowserPropertiesWpfUserControl(BrowserSettings settings)
        {
            InitializeComponent();
            DataContext = settings;
        }
    }
}
