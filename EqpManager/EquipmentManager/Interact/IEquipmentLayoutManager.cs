using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Interact
{
    public interface IEquipmentLayoutManager
    {
        event EventHandler DataInitialized;
        event EventHandler EquipmentDataExported;

        Task Export(IList<EquipmentViewModel> equipments);

        void Synchronize(IList<EquipmentViewModel> equipments);

        void Initialize();
    }
}