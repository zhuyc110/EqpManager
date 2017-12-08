using System.ComponentModel.Composition.Hosting;
using System.Windows;
using EquipmentManager.View;
using Prism.Mef;
using Prism.Regions;

namespace EquipmentManager
{
    public class Bootstrapper : MefBootstrapper
    {
        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
        }

        protected override DependencyObject CreateShell()
        {
            return Container.GetExportedValue<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Container.GetExportedValue<IRegionManager>().RegisterViewWithRegion("MainRegion", typeof(MainView));

            Application.Current.MainWindow?.Show();
        }
    }
}