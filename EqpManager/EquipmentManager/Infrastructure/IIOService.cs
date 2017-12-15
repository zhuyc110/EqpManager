namespace EquipmentManager.Infrastructure
{
    public interface IIOService
    {
        string OpenFileDialog(string title, string filter, bool multiselect);
        void ShowDialog(string title, string message);
        void ShowDialog<TViewModel>(TViewModel viewModel, DialogSetting dialogSetting = null) where TViewModel : DialogViewModel;
        void SetCursorBusy();
        void ReleaseCursor();
    }
}