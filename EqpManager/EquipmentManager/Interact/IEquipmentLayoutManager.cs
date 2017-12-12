using System;
using System.Collections.Generic;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Interact
{
    public interface IEquipmentLayoutManager
    {
        event EventHandler DataInitialized;

        void Export(IList<EquipmentViewModel> equipments);

        void Synchronize(IList<EquipmentViewModel> equipments);

        void Initialize();
    }
}