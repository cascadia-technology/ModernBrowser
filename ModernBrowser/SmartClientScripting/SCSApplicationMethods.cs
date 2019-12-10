using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Messaging;

namespace ModernBrowser.SmartClientScripting
{
    public class SCSApplicationMethods
    {
        public void Maximize()
        {
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ApplicationControlCommand, ApplicationControlCommandData.Maximize));
        }

        public void Minimize()
        {
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ApplicationControlCommand, ApplicationControlCommandData.Minimize));
        }

        public void Close()
        {
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ApplicationControlCommand, ApplicationControlCommandData.Close));
        }

        public void Restore()
        {
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ApplicationControlCommand, ApplicationControlCommandData.Restore));
        }

        public void ReloadConfiguration()
        {
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.SmartClient.ReloadConfigurationCommand));
        }
    }
}