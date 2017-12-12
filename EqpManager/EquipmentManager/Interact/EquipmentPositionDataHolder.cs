using System;
using System.Collections.Generic;

namespace EquipmentManager.Interact
{
    [Serializable]
    public class EquipmentPositionDataHolder
    {
        public List<EquipmentPositionData> EquipmentPositionDatas { get; set; }

        private EquipmentPositionDataHolder()
        {
            EquipmentPositionDatas = new List<EquipmentPositionData>();
        }

        public static EquipmentPositionDataHolder CreateDefault()
        {
            return new EquipmentPositionDataHolder();
        }
    }
}