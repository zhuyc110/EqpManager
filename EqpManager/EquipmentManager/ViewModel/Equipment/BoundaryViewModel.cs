using System.Windows.Controls;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel.Equipment
{
    public class BoundaryViewModel : BindableBase, IEquipmentViewVisibleViewModel
    {
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

        public int Height { get; set; }

        public Orientation Orientation { get; }

        public BoundaryViewModel()
        {
            Height = 100;
        }

        #region Fields

        private int _top = 5;
        private int _left;

        #endregion
    }
}