namespace EquipmentManager.Config
{
    public interface IAppSetting
    {
        string ExportFilePath { get; set; }

        void Save();
    }
}