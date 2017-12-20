using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using EquipmentManager.ViewModel.Equipment;

namespace EquipmentManager.Controls
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += ResizeThumbDragDelta;
            DragCompleted += ResizeThumbDragCompleted;
        }

        #region Private methods

        private void ResizeThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            _resizingBoundaryViewModel.IsResizing = false;
            _resizingBoundaryViewModel = null;
        }

        private void ResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            _resizingBoundaryViewModel = DataContext as BoundaryViewModel;

            if (_resizingBoundaryViewModel != null)
            {
                _resizingBoundaryViewModel.IsResizing = true;
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, _resizingBoundaryViewModel.Size - BOUNDARY_MIN_WIDTH);
                        _resizingBoundaryViewModel.Size -= (int) deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, _resizingBoundaryViewModel.Size - BOUNDARY_MIN_WIDTH);
                        _resizingBoundaryViewModel.Top += (int) deltaVertical;
                        _resizingBoundaryViewModel.Size -= (int) deltaVertical;
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, _resizingBoundaryViewModel.Size - BOUNDARY_MIN_WIDTH);
                        _resizingBoundaryViewModel.Left += (int) deltaHorizontal;
                        _resizingBoundaryViewModel.Size -= (int) deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, _resizingBoundaryViewModel.Size - BOUNDARY_MIN_WIDTH);
                        _resizingBoundaryViewModel.Size -= (int) deltaHorizontal;
                        break;
                }
            }

            e.Handled = true;
        }

        #endregion

        #region Fields

        private BoundaryViewModel _resizingBoundaryViewModel;
        private const int BOUNDARY_MIN_WIDTH = 25;

        #endregion
    }
}