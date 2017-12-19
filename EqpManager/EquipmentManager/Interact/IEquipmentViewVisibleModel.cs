using System.Windows.Controls;

namespace EquipmentManager.Interact
{
    public interface IEquipmentViewVisibleModel
    {
        string Id { get; }

        int Top { get; set; }

        int Left { get; set; }

        int Size { get; set; }

        bool IsEquipment { get; }

        Orientation Orientation { get; }
    }
}