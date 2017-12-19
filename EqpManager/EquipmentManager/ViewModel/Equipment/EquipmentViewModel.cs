using System;
using EquipmentManager.Interact;
using Prism.Mvvm;

namespace EquipmentManager.ViewModel.Equipment
{
    public class EquipmentViewModel : BindableBase, IEquipmentViewVisibleModel
    {
        public string Id => EquipmentId;

        public string EquipmentId { get; set; }

        public string PackageCode
        {
            get => _packageCode;
            set => SetProperty(ref _packageCode, value);
        }

        public int Size { get; set; }

        public bool IsEquipment => true;

        public string StatusChangedTime => _statusChangedTime.ToShortTimeString();

        public string StatusChangedDetailTime => $"{_statusChangedTime.ToShortDateString()} {_statusChangedTime.ToShortTimeString()}";

        public EquipmentStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

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

        public EquipmentViewModel(string equipmentId, int height = 56)
        {
            EquipmentId = equipmentId;
            _statusChangedTime = DateTime.Now;
            Size = height;
        }

        #region Fields

        private int _top = 5;
        private int _left;

        private DateTime _statusChangedTime;

        private EquipmentStatus _status;

        private string _packageCode;

        #endregion
    }
}