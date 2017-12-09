using System.ComponentModel.Composition;
using EquipmentManager.Infrastructure;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.View
{
    /// <summary>
    /// AddEquipmentView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(AddEquipmentView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AddEquipmentView : IView<AddEquipmentViewModel>
    {
        public AddEquipmentViewModel ViewModel
        {
            set => DataContext = value;
        }

        public string Title => "添加设备";

        public AddEquipmentView()
        {
            InitializeComponent();
        }
        
    }
}