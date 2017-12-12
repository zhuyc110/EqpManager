namespace EquipmentManager.Config
{
    public interface IAppSetting
    {
        string ExportFilePath { get; }

        void SetExportFilePath(string filePath);
    }
}