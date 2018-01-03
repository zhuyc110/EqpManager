using System.Windows;
using System.Windows.Controls;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Resources
{
    public class EquipmentViewItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BoundaryDataTemplate { get; set; }

        public DataTemplate EquipmentDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is EquipmentViewModel)
            {
                return EquipmentDataTemplate;
            }
            if (item is BoundaryViewModel)
            {
                return BoundaryDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}