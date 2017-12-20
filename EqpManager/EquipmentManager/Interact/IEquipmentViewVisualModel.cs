using System.Windows.Controls;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Interact
{
    /// <summary>
    /// Data extract of the <see cref="EquipmentViewModel"/> and <see cref="BoundaryViewModel"/>, which is used for serialization.
    /// </summary>
    public interface IEquipmentViewVisualModel
    {
        string Id { get; }

        int Top { get; set; }

        int Left { get; set; }

        int Size { get; set; }

        bool IsEquipment { get; }

        Orientation Orientation { get; }
    }
}