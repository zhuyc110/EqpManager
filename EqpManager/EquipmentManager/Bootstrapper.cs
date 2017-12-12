using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows;
using EquipmentManager.Config;
using EquipmentManager.Properties;
using EquipmentManager.View;
using Prism.Mef;
using Prism.Regions;

namespace EquipmentManager
{
    public class Bootstrapper : MefBootstrapper
    {
        #region Protected methods

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();
            AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(Bootstrapper).Assembly));
        }

        protected override void ConfigureContainer()
        {
            Container.ComposeExportedValue<IAppSetting>(Settings.Default);
            base.ConfigureContainer();
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

        #endregion
    }
}