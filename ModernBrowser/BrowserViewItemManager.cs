using System.ComponentModel;
using VideoOS.Platform.Client;

namespace ModernBrowser
{
    public class BrowserViewItemManager : ViewItemManager
    {
        public BrowserSettings Settings { get; set; }

        public BrowserViewItemManager(string name) : base(name)
        {
            Settings = new BrowserSettings(this);
            Settings.PropertyChanged += SettingsOnPropertyChanged;
        }

        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveProperties();
        }

        public override PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
        {
            return new BrowserPropertiesWpfUserControl(Settings);
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new BrowserViewItemWpfUserControl(Settings);
        }
    }
}