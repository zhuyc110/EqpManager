using System.Windows;
using EquipmentManager.Interact;
using GongSolutions.Wpf.DragDrop;

namespace EquipmentManager.Resources
{
    public class MainViewDragDropHandler : DefaultDragHandler
    {
        public Vector Delta { get; private set; }

        public override void StartDrag(IDragInfo dragInfo)
        {
            base.StartDrag(dragInfo);

            var draggedItem = dragInfo.Data as IEquipmentViewVisibleModel;
            if (draggedItem == null)
            {
                return;
            }
            Delta = dragInfo.DragStartPosition - new Point(draggedItem.Left, draggedItem.Top);
        }
    }
}