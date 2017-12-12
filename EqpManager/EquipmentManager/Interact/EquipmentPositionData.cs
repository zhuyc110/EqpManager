using System;

namespace EquipmentManager.Interact
{
    [Serializable]
    public class EquipmentPositionData
    {
        public string EquipmentId { get; set; }

        public int Top { get; set; }

        public int Left { get; set; }

        public int Size { get; set; }
    }
}