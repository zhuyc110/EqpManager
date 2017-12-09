namespace EquipmentManager.Infrastructure
{
    public interface IIOService
    {
        void ShowDialog<TViewModel>(TViewModel viewModel, DialogSetting dialogSetting = null) where TViewModel : DialogViewModel;
    }
}