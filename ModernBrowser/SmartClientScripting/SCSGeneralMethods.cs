using System;
using System.Linq;
using System.Windows;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Messaging;

namespace ModernBrowser.SmartClientScripting
{
    public class SCSGeneralMethods
    {
        public void ActivateEvent(string name)
        {
            var ms = new ManagementServer(EnvironmentManager.Instance.MasterSite);
            var id = ms.UserDefinedEventFolder.UserDefinedEvents.FirstOrDefault(ude => ude.Name == name)?.Id;
            if (string.IsNullOrEmpty(id)) throw new PathNotFoundMIPException($"TriggerEvent with name '{name}' not found.");
            var fqid = new FQID(ms.ServerId) {ObjectId = new Guid(id), Kind = Kind.TriggerEvent};
            EnvironmentManager.Instance.PostMessage(new Message(MessageId.Control.TriggerCommand), fqid);
        }
    }
}