using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EquipmentManager.Interact
{
    /// <summary>
    /// Manages all the <see cref="IEquipmentViewVisualModel"/> extracted from the application.
    /// </summary>
    public interface IEquipmentLayoutManager
    {
        event EventHandler DataInitialized;

        event EventHandler EquipmentDataExported;

        Task Export(IList<IEquipmentViewVisualModel> equipments);

        /// <summary>
        /// Synchronizes all the <see cref="IEquipmentViewVisualModel"/> data into the given <paramref name="equipments"/>.
        /// </summary>
        void Synchronize(IList<IEquipmentViewVisualModel> equipments);

        /// <summary>
        /// Serializes the last extracted file into the <see cref="IEquipmentLayoutManager"/>
        /// </summary>
        void Initialize();
    }
}