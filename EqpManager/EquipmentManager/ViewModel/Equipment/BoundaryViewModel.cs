using System.Windows.Controls;
using EquipmentManager.Interact;
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

        public Orientation Orientation { get; }

        public BoundaryViewModel()
        {
            Size = 100;
        }

        public static string GetId()
        {
            return _id++.ToString();
        }

        #region Fields

        private static int _id;

        private int _top = 5;
        private int _left;

        #endregion
    }
}