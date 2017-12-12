namespace EquipmentManager.Infrastructure
{
    public interface IIOService
    {
        string OpenFileDialog(string title, string filter, bool multiselect);
        void ShowDialog<TViewModel>(TViewModel viewModel, DialogSetting dialogSetting = null) where TViewModel : DialogViewModel;
    }
}