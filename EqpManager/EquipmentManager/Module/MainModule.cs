using System.ComponentModel.Composition;
using EquipmentManager.Interact;
using Prism.Mef.Modularity;
using Prism.Modularity;

namespace EquipmentManager.Module
{
    [ModuleExport(typeof(MainModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainModule : IModule
    {
        private readonly IEquipmentLayoutManager _equipmentLayoutManager;

        [ImportingConstructor]
        public MainModule(IEquipmentLayoutManager equipmentLayoutManager)
        {
            _equipmentLayoutManager = equipmentLayoutManager;
        }

        public void Initialize()
        {
            _equipmentLayoutManager.Initialize();
        }
    }
}