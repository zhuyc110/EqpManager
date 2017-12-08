using System.Windows;

namespace EquipmentManager
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App
    {
        #region Protected methods

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootStrapper = new Bootstrapper();
            bootStrapper.Run();
        }

        #endregion
    }
}