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

        public override void PropertiesLoaded()
        {
        }

        public override PropertiesWpfUserControl GeneratePropertiesWpfUserControl()
        {
            return new BrowserPropertiesWpfUserControl(this);
        }

        public override ViewItemWpfUserControl GenerateViewItemWpfUserControl()
        {
            return new BrowserViewItemWpfUserControl(this);
        }
        private void SettingsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveProperties();
        }
    }
}