using System.ComponentModel;
using System.Runtime.CompilerServices;
using ModernBrowser.Annotations;

namespace ModernBrowser
{
    public class BrowserSettings : INotifyPropertyChanged
    {
        private readonly BrowserViewItemManager _vim;
        public string Address
        {
            get => _vim?.GetProperty("Address") ?? "https://www.cascadia.tech";
            set
            {
                _vim?.SetProperty("Address", value);
                OnPropertyChanged();
            }
        }

        public bool ShowAddressBar
        {
            get => bool.Parse(_vim?.GetProperty("ShowAddressBar") ?? "false");
            set
            {
                _vim?.SetProperty("ShowAddressBar", value.ToString());
                OnPropertyChanged();
            }
        }

        public BrowserSettings()
        {
            // Parameterless constructer only exists so that the WPF user controls
            // can set their DataContext to an instance of BrowserSettings for
            // design time.
        }

        public BrowserSettings(BrowserViewItemManager vim)
        {
            _vim = vim;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}