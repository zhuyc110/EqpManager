using System.Linq;
using System.Windows;
using EquipmentManager.Interact;
using EquipmentManager.ViewModel.Equipment;
using GongSolutions.Wpf.DragDrop;

namespace EquipmentManager.Resources
{
    public class MainViewDragDropHandler : DefaultDragHandler
    {
        public Vector Delta { get; private set; }

        public override bool CanStartDrag(IDragInfo dragInfo)
        {
            var source = dragInfo.SourceItems.OfType<BoundaryViewModel>().FirstOrDefault();
            if (source != null)
            {
                // Do not enable drag & drop function when boundary is resizing.
                if (source.IsResizing)
                {
                    return false;
                }
                return true;
            }

            return base.CanStartDrag(dragInfo);
        }

        public override void StartDrag(IDragInfo dragInfo)
        {
            base.StartDrag(dragInfo);

            var draggedItem = dragInfo.Data as IEquipmentViewVisualModel;
            if (draggedItem == null)
            {
                return;
            }
            Delta = dragInfo.DragStartPosition - new Point(draggedItem.Left, draggedItem.Top);
        }
    }
}