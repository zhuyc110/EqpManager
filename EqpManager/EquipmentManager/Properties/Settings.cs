using EquipmentManager.Config;

namespace EquipmentManager.Properties
{
    internal sealed partial class Settings : IAppSetting
    {
        #region IAppSetting Members

        public void SetExportFilePath(string filePath)
        {
            this["ExportFilePath"] = filePath;
            Save();
        }

        #endregion
    }
}