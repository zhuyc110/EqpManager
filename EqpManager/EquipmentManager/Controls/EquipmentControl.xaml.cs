using System.Windows;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Controls
{
    /// <summary>
    /// EquipmentControl.xaml 的交互逻辑
    /// </summary>
    public partial class EquipmentControl
    {
        public string EquipmentId
        {
            get => (string) GetValue(EquipmentIdProperty);
            set => SetValue(EquipmentIdProperty, value);
        }

        public string StatusChangeTime
        {
            get => (string) GetValue(StatusChangeTimeProperty);
            set => SetValue(StatusChangeTimeProperty, value);
        }

        public string PackageCode
        {
            get => (string) GetValue(PackageCodeProperty);
            set => SetValue(PackageCodeProperty, value);
        }

        public EquipmentStatus Status
        {
            get => (EquipmentStatus) GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        public bool IsSelected
        {
            get => (bool) GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public EquipmentControl()
        {
            InitializeComponent();
        }

        #region Fields

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof(bool), typeof(EquipmentControl), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty PackageCodeProperty = DependencyProperty.Register(
            "PackageCode", typeof(string), typeof(EquipmentControl), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status", typeof(EquipmentStatus), typeof(EquipmentControl), new PropertyMetadata(default(EquipmentStatus)));

        public static readonly DependencyProperty StatusChangeTimeProperty = DependencyProperty.Register(
            "StatusChangeTime", typeof(string), typeof(EquipmentControl), new PropertyMetadata("00:00"));

        public static readonly DependencyProperty EquipmentIdProperty = DependencyProperty.Register(
            nameof(EquipmentId), typeof(string), typeof(EquipmentControl), new PropertyMetadata(default(string)));

        #endregion
    }
}