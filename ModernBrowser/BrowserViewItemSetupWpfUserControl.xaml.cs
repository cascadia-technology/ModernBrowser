using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModernBrowserShim;

namespace ModernBrowser
{
    public partial class BrowserViewItemSetupWpfUserControl
    {
        private BrowserHost _browser;

        public BrowserViewItemSetupWpfUserControl(BrowserSettings settings, BrowserHost browser = null)
        {
            InitializeComponent();
            DataContext = settings;
            _browser = browser;
        }

        private void BrowserViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireClickEvent();
            }
        }

        private void BrowserViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FireDoubleClickEvent();
            }
        }
    }
}
