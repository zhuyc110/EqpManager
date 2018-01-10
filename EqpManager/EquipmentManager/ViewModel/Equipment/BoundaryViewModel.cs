using System.Windows.Controls;
using EquipmentManager.Interact;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel.Equipment
{
    public class BoundaryViewModel : BindableBase, IEquipmentViewVisualModel, IEquipmentViewItem
    {
        public string Id { get; set; }

        public int Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }

        public int Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }

        public int Size
        {
            get => _size;
            set
            {
                SetProperty(ref _size, value);
                RefreshData();
            }
        }

        public bool IsEquipment => false;

        public Orientation Orientation
        {
            get => _orientation;
            private set => SetProperty(ref _orientation, value);
        }

        public int Width
        {
            get => _width;
            private set => SetProperty(ref _width, value);
        }

        public int Height
        {
            get => _height;
            private set => SetProperty(ref _height, value);
        }

        public bool IsResizing { get; set; }

        public bool CanResizeVertical => Orientation == Orientation.Vertical;

        public bool CanResizeHorizontal => Orientation == Orientation.Horizontal;

        public BoundaryViewModel(Orientation orientation, int size)
        {
            Orientation = orientation;
            Size = size;
            RefreshData();
        }

        public void ChangeOrientation()
        {
            Orientation = Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
            RefreshData();
            RaisePropertyChanged(nameof(CanResizeVertical));
            RaisePropertyChanged(nameof(CanResizeHorizontal));
        }

        public static string GetId()
        {
            return _id++.ToString();
        }

        #region Private methods

        private void RefreshData()
        {
            if (Orientation == Orientation.Horizontal)
            {
                Width = Size;
                Height = 4;
            }
            else
            {
                Width = 4;
                Height = Size;
            }
        }

        #endregion

        #region Fields

        private static int _id;

        private int _top = 5;
        private int _left;
        private Orientation _orientation;
        private int _width = 2;
        private int _height = 2;
        private int _size = 50;

        #endregion
    }
}