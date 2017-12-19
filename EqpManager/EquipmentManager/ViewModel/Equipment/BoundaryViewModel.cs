using System.Windows.Controls;
using System.Windows.Input;
using EquipmentManager.Interact;
using Prism.Commands;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel.Equipment
{
    public class BoundaryViewModel : BindableBase, IEquipmentViewVisibleModel
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

        public int Size { get; set; }

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

        public ICommand ChangeOrientationCommand { get; }

        public BoundaryViewModel(Orientation orientation, int size)
        {
            Orientation = orientation;
            Size = size;
            ChangeOrientationCommand = new DelegateCommand(ExecuteChangeOrientation);
            RefreshData();
        }

        public static string GetId()
        {
            return _id++.ToString();
        }

        #region Private methods

        private void ExecuteChangeOrientation()
        {
            Orientation = Orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;
            RefreshData();
        }

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

        #endregion
    }
}