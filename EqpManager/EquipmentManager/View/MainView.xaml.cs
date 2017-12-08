using System.ComponentModel.Composition;
using EquipmentManager.Infrastructure;
using EquipmentManager.ViewModel;

namespace EquipmentManager.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(MainView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainView : IView<MainViewModel>
    {
        [Import]
        public MainViewModel ViewModel
        {
            set => DataContext = value;
        }

        public string Title => "主界面";

        [ImportingConstructor]
        public MainView()
        {
            InitializeComponent();
        }
    }
}