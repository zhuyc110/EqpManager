using System.Collections.Generic;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Interact
{
    public interface IEquipmentPositionManager
    {
        void Export(IList<EquipmentViewModel> equipments);

        void Synchronize(IList<EquipmentViewModel> equipments);
    }
}