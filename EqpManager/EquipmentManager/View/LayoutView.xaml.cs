using System.ComponentModel.Composition;
using EquipmentManager.Infrastructure;
using EquipmentManager.ViewModel;

namespace EquipmentManager.View
{
    /// <summary>
    /// LayoutView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(LayoutView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LayoutView : IView<LayoutViewModel>
    {
        public LayoutViewModel ViewModel
        {
            set => DataContext = value;
        }

        public string Title => "Set layout";

        public LayoutView()
        {
            InitializeComponent();
        }
    }
}