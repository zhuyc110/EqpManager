using System.Configuration;
using System.Diagnostics;
using EquipmentManager.Config;

namespace EquipmentManager.Properties
{
    internal sealed partial class Settings : IAppSetting
    {
        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("C:\\")]
        public string ExportFilePath
        {
            get => ((string) (this["ExportFilePath"]));
            set => this["ExportFilePath"] = value;
        }
    }
}