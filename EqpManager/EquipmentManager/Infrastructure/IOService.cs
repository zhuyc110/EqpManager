using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Practices.ServiceLocation;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace EquipmentManager.Infrastructure
{
    [Export(typeof(IIOService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class IOService : IIOService
    {
        [ImportingConstructor]
        public IOService(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        #region IIOService Members

        public string OpenFileDialog(string title, string filter, bool multiselect)
        {
            using (var ofd = new OpenFileDialog {Filter = filter, Multiselect = multiselect, Title = title})
            {
                var result = ofd.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return string.Empty;
        }

        public void ShowDialog(string title, string message)
        {
            MessageBox.Show(message, title);
        }

        public void ShowDialog<TViewModel>(TViewModel viewModel, DialogSetting dialogSetting = null) where TViewModel : DialogViewModel
        {
            var view = _serviceLocator.GetInstance<IView<TViewModel>>();
            view.ViewModel = viewModel;
            ShowView(viewModel, view, dialogSetting);
        }

        public void SetCursorBusy()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        }

        public void ReleaseCursor()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            });
        }

        #endregion

        #region Private methods

        private static void ShowView<TViewModel>(TViewModel viewModel, IView<TViewModel> view, DialogSetting dialogSetting = null) where TViewModel : DialogViewModel
        {
            var window = new Window
            {
                Owner = Application.Current.MainWindow,
                Title = view.Title,
                Content = view,
                Width = dialogSetting?.Width ?? 300,
                Height = dialogSetting?.Height ?? 200,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                WindowState = dialogSetting?.WindowState??WindowState.Normal
            };
            viewModel.RequestClose += (sender, args) => window.Close();
            window.Closed += (sender, args) => viewModel.Dispose();
            window.ShowDialog();
        }

        #endregion

        #region Fields

        private readonly IServiceLocator _serviceLocator;

        #endregion
    }
}