using System;
using System.Windows.Controls;

namespace EquipmentManager.Interact
{
    [Serializable]
    public class EquipmentPositionData : IEquipmentViewVisualModel
    {
        public string Id { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Size { get; set; }

        public bool IsEquipment { get; set; }

        public Orientation Orientation { get; set; }
    }
}