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
            get => _vim?.GetProperty("Address") ?? string.Empty;
            set
            {
                _vim?.SetProperty("Address", value);
                OnPropertyChanged();
            }
        }

        public bool ShowNavigationBar
        {
            get => bool.Parse(_vim?.GetProperty("ShowNavigationBar") ?? "false");
            set
            {
                _vim?.SetProperty("ShowNavigationBar", value.ToString());
                OnPropertyChanged();
            }
        }

        public bool EnableSmartClientScripting
        {
            get => bool.Parse(_vim?.GetProperty("EnableSmartClientScripting") ?? "false");
            set
            {
                _vim?.SetProperty("EnableSmartClientScripting", value.ToString());
                OnPropertyChanged();
            }
        }

        public float DpiScaleFactor
        {
            get => float.Parse(_vim?.GetProperty("DpiScaleFactor") ?? "100");
            set
            {
                if (value < 25) value = 25;
                if (value > 500) value = 500;
                _vim.SetProperty("DpiScaleFactor", value.ToString());
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