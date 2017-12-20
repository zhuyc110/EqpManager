using EquipmentManager.View;

namespace EquipmentManager.ViewModel.Equipment
{
    /// <summary>
    /// Item which is presented in <see cref="MainView"/>, properties indicate if the item can be resized.
    /// </summary>
    public interface IEquipmentViewItem
    {
        bool CanResizeVertical { get; }

        bool CanResizeHorizontal { get; }
    }
}