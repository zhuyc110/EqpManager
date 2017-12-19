using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentManager.Interact
{
    public interface IEquipmentLayoutManager
    {
        event EventHandler DataInitialized;
        event EventHandler EquipmentDataExported;

        Task Export(IList<IEquipmentViewVisibleModel> equipments);

        void Synchronize(IList<IEquipmentViewVisibleModel> equipments);

        void Initialize();
    }
}